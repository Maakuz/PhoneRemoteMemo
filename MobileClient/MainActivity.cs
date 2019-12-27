using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android;
using System.Collections.Generic;

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
            TextView recieved = FindViewById<TextView>(Resource.Id.text_recieved);
            Button sendbutton = FindViewById<Button>(Resource.Id.btn_send);
            Button updateButton = FindViewById<Button>(Resource.Id.btn_update);
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner_ips);

            updateIPs(spinner);

            updateButton.Click += (sender, e) =>
            {
                updateIPs(spinner);
            };
            

            sendbutton.Click += (sender, e) =>
            {
                m_client = new Client(5000, spinner.SelectedItem.ToString());

                string recievedMessage = m_client.sendMessage(message.Text);
                if (string.IsNullOrWhiteSpace(recievedMessage))
                {
                    recieved.Text = string.Empty;
                }
                else
                {
                    recieved.Text = recievedMessage;
                }
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void updateIPs(Spinner spinner)
        {
            string[] ips = { "192.168.43.171"};

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ips);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }
    }
}