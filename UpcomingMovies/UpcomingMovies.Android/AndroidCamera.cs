using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using UpcomingMovies.Dependency;
using UpcomingMovies.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidCamera))]
namespace UpcomingMovies.Droid
{

    public class AndroidCamera : ICameraOCR
    {
        public string ReadTextFromImage(string file_path)
        {
            var text = string.Empty;
            var bitMap = BitmapFactory.DecodeFile(file_path);

            var textRecognizer = new TextRecognizer.Builder(Application.Context).Build();

            if (!textRecognizer.IsOperational)
            {
                return text;
            }

            Frame frame = new Frame.Builder().SetBitmap(bitMap).Build();
            SparseArray items = textRecognizer.Detect(frame);
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < items.Size(); i++)
            {
                TextBlock item = (TextBlock)items.ValueAt(i);
                strBuilder.Append(item.Value);
                strBuilder.Append(" ");
            }
            text = strBuilder.ToString();

            return text;
        }
    }
}