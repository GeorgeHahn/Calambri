using System;
using System.Threading;
using System.Threading.Tasks;
using Calambri.Interfaces;
using LibUsbDotNet;
using LibUsbDotNet.Main;

namespace Calambri.Core
{
    public class USBFadecandy: IFadecandyDevice
    {
        private byte[] activeBuffer;
        private byte[] queuedBuffer;
        private bool dirty;
        private UsbDevice fcdevice;
        private UsbEndpointWriter writer;
        
        public USBFadecandy(int pixelCount = 0, string serialNumber = "") : base(pixelCount)
        {
            if(pixelCount > 512)
                throw new ArgumentOutOfRangeException("pixelCount");

            var packets = pixelCount/21; // Can fit 21 pixels into each packet
            activeBuffer = new byte[64 * packets]; // Each packet needs to be 64 bytes
            queuedBuffer = new byte[64 * packets];

            // Setup control bytes for all packets. Never wiped out, only needs to be done once.
            for (int i = 0; i < packets; i++)
            {
                byte packet_index = (byte)(i & 0x1F); // Bits 0-4, packet index
                byte final = (byte)(i == packets - 1 ? 1 : 0); // Bit 5, final bit (denotes last packet)
                //byte type = 0; // Bits 6-7, type code (0 for video data)

                activeBuffer[i*64] = (byte)(packet_index & (final >> 5)); // First byte in each packet is a bitfield
            }
            Array.Copy(activeBuffer, queuedBuffer, activeBuffer.Length);

            dirty = false;

            UsbDeviceFinder usbFinder = string.IsNullOrEmpty(serialNumber) ? new UsbDeviceFinder(0x1d50, 0x607a) : new UsbDeviceFinder(0x1d50, 0x607a, serialNumber);

            ErrorCode ec = ErrorCode.None;
            
            // Find and open the usb device.
            fcdevice = UsbDevice.OpenUsbDevice(usbFinder);
                
            // If the device is open and ready
            if (fcdevice == null) throw new Exception("Device Not Found.");

            // If this is a "whole" usb device (libusb-win32, linux libusb)
            // it will have an IUsbDevice interface. If not (WinUSB) the 
            // variable will be null indicating this is an interface of a 
            // device.
            IUsbDevice wholeUsbDevice = fcdevice as IUsbDevice;
            if (!ReferenceEquals(wholeUsbDevice, null))
            {
                // This is a "whole" USB device. Before it can be used, 
                // the desired configuration and interface must be selected.

                // Select config #1
                wholeUsbDevice.SetConfiguration(1);

                // Claim interface #0.
                wholeUsbDevice.ClaimInterface(0);
            }

            // open write endpoint 1.
            writer = fcdevice.OpenEndpointWriter(WriteEndpointID.Ep01);
        }

        private int PixelToIndex(int pixel)
        {
            var packetindex = pixel/21; // 21 pixels per packet
            var indexinpacket = pixel%21;
            return (packetindex*64) + 1 + indexinpacket; // 64 bytes per packet, one byte of header within each packet
        }

        public override Color GetPixel(int pixelNum)
        {
            if (pixelNum > PixelCount)
                throw new ArgumentOutOfRangeException("pixelNum");

            var firstbyte = PixelToIndex(pixelNum);
            return new Color(
                queuedBuffer[firstbyte],
                queuedBuffer[firstbyte + 1],
                queuedBuffer[firstbyte + 2]);
        }

        public override void SetPixel(int pixelNum, Color c)
        {
            if (pixelNum > PixelCount)
                throw new ArgumentOutOfRangeException("pixelNum");

            var firstbyte = PixelToIndex(pixelNum);
            queuedBuffer[firstbyte] = c.R;
            queuedBuffer[firstbyte + 1] = c.G;
            queuedBuffer[firstbyte + 2] = c.B;
            dirty = true;
        }

        public override async void Commit()
        {
            // This probably breaks the fadecandy interpolation algorithms
            //if(!dirty)
            //    return;

            // Filp buffers
            var queued = queuedBuffer;
            queuedBuffer = activeBuffer;
            activeBuffer = queued;

            // Send active buffer to device
            await Task.Run(() =>
            {
                int transferredOut;
                UsbTransfer usbWriteTransfer;
                
                // Create and submit transfer
                ErrorCode ecWrite = writer.SubmitAsyncTransfer(activeBuffer, 0, activeBuffer.Length, 100, out usbWriteTransfer);
                if (ecWrite != ErrorCode.None)
                {
                    throw new Exception("Submit async write Failed.");
                }

                WaitHandle.WaitAll(new[] { usbWriteTransfer.AsyncWaitHandle, }, 200, false);
                if (!usbWriteTransfer.IsCompleted) usbWriteTransfer.Cancel();

                ecWrite = usbWriteTransfer.Wait(out transferredOut);

                if (ecWrite != ErrorCode.None)
                {
                    throw new Exception("Wait async write failed");
                }

                usbWriteTransfer.Dispose();
            });
        }

        // TODO: Dispose function to release USB device
    }
}