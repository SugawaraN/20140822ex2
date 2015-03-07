using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Kinect;
using OpenCvSharp;
using System.Drawing.Imaging;
using System.Threading;
using OpenCvSharp.Extensions;
using OpenCvSharp.CPlusPlus;
using CPP = OpenCvSharp.CPlusPlus;
using System.Windows;
using System.Runtime.InteropServices;
using System.IO;
using CenterOfGravity;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public const int Bgr32BytesPerPixel = 4;    //ピクセルあたりのバイト数
        const string COLOR = "_color";
        const string DEPTH = "_depth";
        const string SKELETON = "_skeleton";
        const string COLOR_TO_DEPTH = "_point";
        const string PRESSURE = "";
        IplImage image;

        int saveNumber = 0;
        int count = 0;
        const int CountMax = 100;
        const int DepthMax = 4000;
        DirectoryInfo di;
        string filename = "";
        
        /*----被験者-----------*/
        

        string subject = "test";
        string condition = "0";


        /*---------------------*/

        //深度データ
        bool saving = false;
        bool output = false;
        int[] depth;
        //貯める
        long[] save_timestamp; //タイムスタンプ
        //color
        int[,] save_depth;  //depth
        DepthImagePoint[,] save_colorToDepth; //colorToDepth
        DepthImagePoint[,] save_skeleton; //skeletonToDepth
        Save save_pressure; //pressure
        
        public Form1()
        {
            InitializeComponent();
            if (KinectSensor.KinectSensors.Count < 1)
            {
                Console.WriteLine("kinectをもっと接続してください");
            }
            //ファイル名
            DateTime today = DateTime.Now;
            string mo, d, h, m, s;
            if (today.Month < 10)
                mo = "0" + today.Month.ToString();
            else mo = today.Month.ToString();
            if (today.Day < 10)
                d = "0" + today.Day.ToString();
            else d = today.Day.ToString();
            if (today.Hour < 10)
                h = "0" + today.Hour.ToString();
            else h = today.Hour.ToString();
            if (today.Minute < 10)
                m = "0" + today.Minute.ToString();
            else m = today.Minute.ToString();
            if (today.Second < 10)
                s = "0" + today.Second.ToString();
            else s = today.Second.ToString();
            filename = today.Year.ToString() + mo + d + h + m + s;
            //被験者名・実験条件はあとで足す

            //ディレクトリ作成
            di = Directory.CreateDirectory(filename);

            image = Cv.CreateImage(new CvSize(640, 480), BitDepth.U8, 1);
            save_timestamp = new long[CountMax];
            save_depth = new int[CountMax, 640 * 480];
            save_skeleton = new DepthImagePoint[CountMax, 20];
            save_colorToDepth = new DepthImagePoint[CountMax, 640 * 480];
            save_pressure = new Save("COM8");
            Thread threadStart = new Thread(new ParameterizedThreadStart(thrStart));
            threadStart.Start();
            threadStart.Join();
            //KinectSensor.KinectSensors[0].AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(kinect_AllFramesReady);
        }

        private void thrStart(object o)
        {
            startKinect(KinectSensor.KinectSensors[0], 0);
        }

        private void startKinect(KinectSensor kinect, int kinectNumber)
        {
            //kinect.DepthStream.Range = DepthRange.Near;
            kinect.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
            kinect.ColorStream.Enable();
            kinect.DepthStream.Enable();
            kinect.SkeletonStream.Enable();
            kinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(kinect_AllFramesReady);
            kinect.Start();
        }

        unsafe void kinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            try
            {
                KinectSensor kinectSensor = sender as KinectSensor;
                if (kinectSensor == null) return;
                using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
                using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
                using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
                {
                    if (colorFrame != null && depthFrame != null && skeletonFrame != null)
                    {
                        /* タイムスタンプ */
                        long timestamp = depthFrame.Timestamp;
                        //Console.WriteLine(timestamp);

                        /* RGBデータ取得 */
                        byte[] colorPixel = new byte[colorFrame.PixelDataLength];
                        colorFrame.CopyPixelDataTo(colorPixel);
                        IplImage srcImg = IplImage.FromPixelData(new CvSize(colorFrame.Width, colorFrame.Height), 4, colorPixel);

                        /* 距離データを取得 */
                        DepthImagePixel[] dip = new DepthImagePixel[depthFrame.PixelDataLength];
                        depthFrame.CopyDepthImagePixelDataTo(dip);

                        /* 距離データのみ保存 */
                        short[] depthPixel = new short[depthFrame.PixelDataLength];
                        depthFrame.CopyPixelDataTo(depthPixel);

                        /* 距離に変換 */
                        int[] distance = new int[depthFrame.PixelDataLength];
                        for (int i = 0; i < depthPixel.Length; i++)
                        {
                            distance[i] = dip[i].Depth;
                            
                        }
                        

                        /* RGB画像を深度画像にマッピング */
                        DepthImagePoint[] colorPoint = new DepthImagePoint[colorFrame.PixelDataLength / colorFrame.BytesPerPixel];
                        DepthImageFormat dp = DepthImageFormat.Resolution640x480Fps30;
                        kinectSensor.CoordinateMapper.MapColorFrameToDepthFrame(ColorImageFormat.RgbResolution640x480Fps30,
                                                                                dp,
                                                                                dip, colorPoint);

                        /* 距離データ画像化*/
                        toIplImage(distance, depthFrame.Width, depthFrame.Height, kinectSensor.UniqueKinectId);
                        

                        /* スケルトン */
                        Skeleton[] skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                        skeletonFrame.CopySkeletonDataTo(skeletons);
                        DepthImagePoint[] jointPoints = new DepthImagePoint[skeletons[0].Joints.Count];
                        if (skeletons[0].TrackingState == SkeletonTrackingState.Tracked)
                        {
                            /* 1人目のプレイヤーについて関節取得 */
                            Joint[] joints = new Joint[skeletons[1].Joints.Count];

                            byte* p = (byte*)image.ImageData;
                            foreach (JointType jointType in Enum.GetValues(typeof(JointType)))
                            {
                                int i = (int)jointType;
                                jointPoints[i] = kinectSensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skeletons[0].Joints[jointType].Position, DepthImageFormat.Resolution640x480Fps30);

                            }
                            this.BackColor = Color.Black;

                        }
                        else
                        {
                            this.BackColor = Color.White;
                        }

                        /* セーブ中だったら */
                         if (saving)
                        {
                            if (count < CountMax)
                            {
                                Console.WriteLine(count);
                                save_timestamp[count] = timestamp;//timestamp
                                Cv.SaveImage(@di.FullName + "\\" + filename + subject + condition /* + "_" + saveNumber.ToString() */ +COLOR + "_" + count.ToString() + ".bmp", srcImg);//color
                                for (int i = 0; i < 640*480; i++)
                                {
                                    save_depth[count, i] = distance[i]; //depth
                                    save_colorToDepth[count, i] = colorPoint[i];//colorToDepth
                                }
                                for (int i = 0; i < jointPoints.Count(); i++)
                                {
                                    save_skeleton[count, i] = jointPoints[i];//skeleton
                                }
                                
                                save_pressure.memData();//pressure

                                count++;
                            }
                            else
                            {
                                saving = false;
                                /*
                                 *  スレッド・セーブ
                                 *  データアウトプット
                                 * 
                                 */
                                Thread threadSave = new Thread(new ParameterizedThreadStart(thrSave));
                                threadSave.Start();
                                count = 0;
                            }
                        }

                        pictureBoxDepth.Image = image.ToBitmap();
                        
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("framerady"+ ex.Message);
            }
            
        }

        private void thrSave(object o)
        {
            /*
             
               save処理
             
             
             */
            //color 都度セーブしてるので今回しない
            //depth
            Thread threadSaveDepth = new Thread(new ParameterizedThreadStart(thrSaveDepth));
            //skeleton
            Thread threadSaveSkeleton = new Thread(new ParameterizedThreadStart(thrSaveSkeleton));
            //colorToDepth
            Thread threadSaveColorToDepth = new Thread(new ParameterizedThreadStart(thrSaveColorToDepth));
            //pressure
            Thread threadSavePressure = new Thread(new ParameterizedThreadStart(thrSavePressure));
            Console.WriteLine("save start.");
            threadSaveDepth.Start();
            threadSaveSkeleton.Start();
            threadSaveColorToDepth.Start();
            threadSavePressure.Start();

            threadSaveDepth.Join();
            threadSaveSkeleton.Join();
            threadSaveColorToDepth.Join();
            threadSavePressure.Join();

            saveNumber++;
        }

        private void thrSaveDepth(object o)
        {
            DataTableToCsv(save_depth, @di.FullName + "\\" + filename + subject + condition /* + "_" + saveNumber.ToString() */ + DEPTH, save_timestamp);
            Console.WriteLine("depth");
        }
        private void thrSaveSkeleton(object o)
        {
            DataTableToCsv(save_skeleton, @di.FullName + "\\" + filename + subject + condition /* + "_" + saveNumber.ToString() */ + SKELETON, save_timestamp);
            Console.WriteLine("skeleton");

        }
        private void thrSaveColorToDepth(object o)
        {
            DataTableToCsv(save_colorToDepth, @di.FullName + "\\" + filename + subject + condition /* + "_" + saveNumber.ToString() */ + COLOR_TO_DEPTH, save_timestamp);
            Console.WriteLine("color to depth");
        }
        private void thrSavePressure(object o)
        {
            save_pressure.saveData(@di.FullName + "\\" + filename + subject + condition /* + "_" + saveNumber.ToString() */ + PRESSURE);
            Console.WriteLine("pressure");
        }

        static public void DataTableToCsv(int[,] dt, string filePath, long[] timestamp)
        {
            StringBuilder sb = new StringBuilder();
            System.IO.StreamWriter sw = null;
            List<int> filterIndex = new List<int>();

            try
            {
                sw = new System.IO.StreamWriter(filePath + ".csv", false, System.Text.Encoding.GetEncoding("Shift_JIS"));

                //----------------------------------------------------------//
                // 内容を出力します。                                       //
                //----------------------------------------------------------//
                for (int j=0; j<CountMax;j++)
                {
                    //タイムスタンプ
                    sb.Append(timestamp[j].ToString());
                    sb.Append(",");
                    //中身
                    for (int i = 0; i < 640*480; i++)
                    {
                        sb.Append(dt[j,i].ToString().Replace("\"", "\"\""));
                        sb.Append(",");
                    }
                    sb.Append("\r");
                    sw.Write(sb.ToString());
                    sb.Clear();
                }
                
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }
        static public void DataTableToCsv(DepthImagePoint[,] dt, string filePath, long[] timestamp)
        {
            StringBuilder sb = new StringBuilder();
            System.IO.StreamWriter sw = null;
            List<int> filterIndex = new List<int>();

            try
            {
                sw = new System.IO.StreamWriter(filePath + ".csv", false, System.Text.Encoding.GetEncoding("Shift_JIS"));

                //----------------------------------------------------------//
                // 内容を出力します。                                       //
                //----------------------------------------------------------//
                for (int j = 0; j < CountMax; j++)
                {
                    //タイムスタンプ
                    sb.Append(timestamp[j].ToString());
                    sb.Append(",");
                    //中身
                    int length = dt.Length / CountMax;
                    for (int i = 0; i < length; i++)
                    {
                        sb.Append(dt[j, i].X.ToString().Replace("\"", "\"\""));
                        sb.Append(",");
                        sb.Append(dt[j, i].Y.ToString().Replace("\"", "\"\""));
                        sb.Append(",");
                        sb.Append(dt[j, i].Depth.ToString().Replace("\"", "\"\""));
                        sb.Append(",");
                    }
                    sb.Append("\r");
                    sw.Write(sb.ToString());
                    sb.Clear();
                }
                
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        public unsafe void toIplImage(int[] d, int width, int height, string id)
        {
            IplImage imageDepth =  null;
            int[] depth;
            try
            {
                depth = (int[])d.Clone();

                imageDepth = Cv.CreateImage(new CvSize(width, height), BitDepth.U8, 1);
                byte* p = (byte*)imageDepth.ImageData;
                int min, max;

                //とりあえずしょり
                for (int n = 0; n < depth.Length; n++)
                {

                    if (depth[n] > DepthMax || depth[n] < 400)
                    {
                        depth[n] = 0;
                    }
                    else
                    {
                        depth[n] = DepthMax - depth[n];
                    }
                }
                min = depth.Min();
                max = depth.Max();
                //画像化
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {

                        p[y * width + x] = (byte)(255 * depth[y * width + x] / (max - min));

                    }
                }

                image = imageDepth.Clone();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (imageDepth != null) imageDepth.Dispose();
                
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saving = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            subject = textBox1.Text;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            condition = "_" + listBox1.SelectedItem.ToString();
        }

    }
}
