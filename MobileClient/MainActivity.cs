using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Net;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace MobileClient
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Client m_client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Get our UI controls from the loaded layout
            EditText message = FindViewById<EditText>(Resource.Id.input_text);
            EditText recieved = FindViewById<EditText>(Resource.Id.text_recieved);
            Button sendbutton = FindViewById<Button>(Resource.Id.btn_send);
            Button updateButton = FindViewById<Button>(Resource.Id.btn_update);
            Button clearButton = FindViewById<Button>(Resource.Id.btn_clear);
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner_ips);

            List<string> ips = new List<string>{ "192.168.43.171", "192.168.0.52" };

            updateIPs(spinner, ips.ToArray());

            updateButton.Click += (sender, e) =>
            {
                IPAddress trash;
                if (IPAddress.TryParse(message.Text, out trash))
                    ips.Add(message.Text);

                updateIPs(spinner, ips.ToArray());
            };

            clearButton.Click += (sender, e) =>
            {
                recieved.Text = "";
            };

            recieved.LongClick += (sender, e) =>
            {
                recieved.FocusableInTouchMode = !recieved.FocusableInTouchMode;

                if (recieved.FocusableInTouchMode)
                    recieved.RequestFocus();

                recieved.Text += "loooong\r\n";
            };

            sendbutton.Click += (sender, e) =>
            {
                m_client = new Client(5000, spinner.SelectedItem.ToString());

                PingReply reply = m_client.testConnection();

                if (reply.Status == IPStatus.Success)
                    recieved.Text += m_client.sendMessage(message.Text) + "\r\n";

                else
                    recieved.Text += $"No connection could be established to {m_client.m_ip}:{m_client.m_port}\r\n";
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void updateIPs(Spinner spinner, string[] ips)
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ips);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }
    }
}