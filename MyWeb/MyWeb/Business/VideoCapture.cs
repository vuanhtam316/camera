using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
//using lib;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace NFaceID
{
    public class VideoCapture
    {
        [DllImport("CaptureVideo.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void initCapture(out IntPtr cap);

        [DllImport("CaptureVideo.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool openCamera(out IntPtr cap, int index);

        [DllImport("CaptureVideo.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool open(out IntPtr cap, [MarshalAs(UnmanagedType.LPStr)] String file);

        [DllImport("CaptureVideo.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool queryFrame(IntPtr ptr, out IntPtr dst);

        [DllImport("CaptureVideo.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void releaseCapture(out IntPtr ptr);

        [DllImport("CaptureVideo.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool getDimesion_(IntPtr ptr, ref int w, ref int h, ref int channel);

        [DllImport("CaptureVideo.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void releaseImage(out IntPtr src);


        public IntPtr m_capture = new IntPtr();
        public bool isOpen = false;
        public VideoCapture()
        {
            initCapture(out m_capture);
        }
        ~VideoCapture()
        {
            if (isOpen)
            {
                releaseCapture(out m_capture);
                isOpen = false;
            }

        }
        public void Dispose()
        {
            if (isOpen)
            {
                releaseCapture(out m_capture);
                isOpen = false;
            }
        }
        //mo tu webcam
        public bool Open(int index)
        {
            isOpen = openCamera(out m_capture, index);
            return isOpen;
        }
        public bool OpenFromUrl(String url)
        {
            isOpen = open(out m_capture, url);
            return isOpen;
        }
        //mo tu file streaming

        public bool queryFrame(ref Bitmap bmp)
        {
            IntPtr frame;
            bool res = queryFrame(m_capture, out frame);
            if (!res)
                return false;
            if (bmp != null)
                bmp.Dispose();
            bmp = ConvertIntPrToBitmap(frame);
            releaseImage(out frame);
            return res;
        }
        public static Bitmap ConvertIntPrToBitmap(IntPtr ptrImage)
        {
            /*
             * Structure diagram of MIplImage type and OpenCV EmguCV in the IplImage type is the same
             * Only after the conversion, you can use the MIplImage structure of the imageData, height, width, widthStep and other data
             */
            if (ptrImage == null)
                return null;
            int w = 0, h = 0, channel = 0;
            bool res = getDimesion_(ptrImage, ref w, ref h, ref channel);
            if (!res)
                return null;
            Image<Bgr, Byte> image = new Image<Bgr, Byte>(w, h);
            if (channel == 3)
                CvInvoke.cvCopy(ptrImage, image.Ptr, (IntPtr)null);
            else
                CvInvoke.cvCvtColor(ptrImage, image.Ptr, COLOR_CONVERSION.GRAY2BGR);
            if (image == null)
                return null;
            Bitmap a = image.ToBitmap();
            image.Dispose();
            image = null;
            return a;
        }

    }
}
