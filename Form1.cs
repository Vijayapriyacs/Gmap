using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using System.IO;


namespace BusRoutePath
{


    public partial class Form1 : Form
    {

        GMapOverlay routesOverlay = new GMapOverlay("Overlay");
        private List<double> latlist, lnglist;
        private List<string> Combined, Place;
        List<PointLatLng> points;
        List<PointLatLng> seperatePoints;
        TextBox resultFileName;
        double minutes, seconds, tenths, minutes1, seconds1, tenths1;
        private string RouteFileName;
        public Form1()
        {
            InitializeComponent();
            intialmap();
        }
        private void intialmap()
        {

            gMap.MapProvider = GoogleMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gMap.SetPositionByKeywords("India");
            gMap.Zoom = 5;
            gMap.MinZoom = 2;
            gMap.MaxZoom = 100;
            gMap.AutoScroll = true;
            gMap.IsAccessible = true;

            latlist = new List<double>();
            lnglist = new List<double>();
            Combined = new List<string>();
            Place = new List<string>();
            points = new List<PointLatLng>();
           // seperatePoints = new List<PointLatLng>();


        }


        /*   protected override void OnMouseClick(MouseEventArgs e)
           {
               base.OnMouseClick(e);
               if (e.Button == MouseButtons.Left)
               {
                   double lat = gMap.FromLocalToLatLng(e.X, e.Y).Lat;
                   double lng = gMap.FromLocalToLatLng(e.X, e.Y).Lng;

               }
           }
           */

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            gMap.OnMapDrag += new MapDrag(gMap_MarkerDrag);

        }
        private void gMap_MarkerDrag()
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GMarkerGoogle markera = new GMarkerGoogle(new PointLatLng(36.657403, 10.327148), GMarkerGoogleType.blue_pushpin);
            routesOverlay.Markers.Add(markera);
            gMap.Overlays.Add(routesOverlay);

        }
        private void Form1_OnClose()
        {
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void ShowRoute(object sender, EventArgs e)
        {
            PointLatLng start = new PointLatLng(11.1085, 77.3411);
            PointLatLng end = new PointLatLng(11.0168, 76.9558);
            MapRoute route = GoogleMapProvider.Instance.GetRoute(start, end, false, false, 15);
            GMapMarker marker = new GMarkerGoogle(start, GMarkerGoogleType.blue_pushpin);
            GMapMarker marker1 = new GMarkerGoogle(end, GMarkerGoogleType.blue_pushpin);
            routesOverlay.Markers.Add(marker);
            routesOverlay.Markers.Add(marker1);
            GMapRoute r = new GMapRoute(route.Points, "My route");
            gMap.UpdateRouteLocalPosition(r);
            routesOverlay.Routes.Add(r);
            gMap.Overlays.Add(routesOverlay);
            gMap.ZoomAndCenterRoute(r);

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_clicked(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Addpathpoints_Click(object sender, EventArgs e)
        {
            //get gmap location marker location where click and show it on map

        }

        private void source_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //calls the ask filefilename
        }

        private void AskFileName(object sender, MouseEventArgs e)
        {
            Form2 testDialog = new Form2();
            string firstColumn, secondColumn;
            DialogResult res = testDialog.ShowDialog(this);
            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (res == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                resultFileName = new TextBox();
                resultFileName.Text = testDialog.Result;
                RouteFileName = resultFileName.Text.ToString() + ".txt";
                // Write the string to a file.              
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RouteFileName);
                File.Create(filePath).Dispose();
                int count = points.Count;
                foreach (var item in points)
                {

                    minutes = (item.Lat - Math.Floor(item.Lat)) * 60.0;
                    seconds = (minutes - Math.Floor(minutes)) * 60.0;
                    tenths = (seconds - Math.Floor(seconds)) * 10.0;
                    // get rid of fractional part
                    minutes = Math.Floor(minutes);
                    seconds = Math.Floor(seconds);
                    tenths = Math.Floor(tenths);
                    minutes1 = (item.Lng - Math.Floor(item.Lng)) * 60.0;
                    seconds1 = (minutes1 - Math.Floor(minutes1)) * 60.0;
                    tenths1 = (seconds1 - Math.Floor(seconds1)) * 10.0;
                    // get rid of fractional part
                    minutes1 = Math.Floor(minutes1);
                    seconds1 = Math.Floor(seconds1);
                    tenths1 = Math.Floor(tenths1);


                    double lat = item.Lat;
                    double lon = item.Lng;
                    int latdegree = (int)lat;

                    int longdegree = (int)lon;
                    string latDir = (lat >= 0 ? "N" : "S");


                    string lonDir = (lon >= 0 ? "E" : "W");

                    /*    

                        Console.WriteLine(
                            Math.Truncate(lat) + " " + Math.Truncate(latMinPart) + " " + Math.Truncate(latSecPart) + Math.Truncate(lattenthPart)+ " " + latDir
                            );

                        Console.WriteLine(
                            Math.Truncate(lon) + " " + Math.Truncate(lonMinPart) + " " + Math.Truncate(lonSecPart)  +Math.Truncate(lontenthPart) + " " + lonDir
                            );
                    */

                    firstColumn = String.Format("{0}{1}{2}{3}","D:"+ latdegree, "M:"+minutes, "S"+seconds, latDir);
                    secondColumn = String.Format("{0}{1}{2}{3}", "D:"+longdegree,"M"+ minutes1,"S"+ seconds1, lonDir);


                    // Combined.Add(String.Format("{0}{1}{2}{3}",sn, firstColumn, secondColumn,end));
                    Combined.Add("@@" + firstColumn + "," + secondColumn + "#");

                }
                // string location = Place.Count <= i ? "" : Place[i].ToString();



                // file.Write(Combined);"Z:\\" + RouteFileName + ".txt"
                // file.Close();


                // string location = Place.Count <= i ? "" : Place[i].ToString();
                var resToSave = string.Join(",", Combined.ToArray());
                File.WriteAllText(filePath, resToSave);
                gMap.Overlays.Remove(routesOverlay);

                //  seperatePoints.Clear();

                points.Clear();

            }

            else if (res == DialogResult.Cancel)
            {
                testDialog.DialogResult = DialogResult.None;

            }

            testDialog.Dispose();
        }


        private void gMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                double lat = gMap.FromLocalToLatLng(e.X, e.Y).Lat;
                double lng = gMap.FromLocalToLatLng(e.X, e.Y).Lng;
                gMap.Position = new PointLatLng(lat, lng);
                points.Add(new PointLatLng(lat, lng));
              //  seperatePoints.Add(new PointLatLng(lat,lng));
                List<Placemark> plc = null;
                var st = GMapProviders.GoogleMap.GetPlacemarks(gMap.FromLocalToLatLng(e.X, e.Y), out plc);
                if (st == GeoCoderStatusCode.G_GEO_SUCCESS && plc != null)
                {
                    foreach (var pl in plc)
                    {
                        if (!string.IsNullOrEmpty(pl.PostalCodeNumber))
                        {
                       
                            Place.Add(pl.Address);
                        }
                    }
                }

                var marker = new GMarkerGoogle(new PointLatLng(lat, lng), GMarkerGoogleType.arrow);
                routesOverlay.Markers.Add(marker);
                gMap.Overlays.Add(routesOverlay);

            }
        }


        private void button2_Click(object sender, EventArgs e)
        {

            GMapRoute r1 = new GMapRoute(points, "A walk in the park");
            r1.Stroke = new Pen(Color.Red, 3);
            gMap.UpdateRouteLocalPosition(r1);
            routesOverlay.Routes.Add(r1);
            gMap.Overlays.Add(routesOverlay);
            gMap.ZoomAndCenterRoute(r1);


        }
    }
}
