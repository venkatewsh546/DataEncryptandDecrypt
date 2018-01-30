using System;
using Android.Hardware.Fingerprints;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Widget;
using Android;
using Android.Runtime;
using Java.Lang;

namespace DataEncryptAndDecrypt
{
    internal class FingerprintHandler:FingerprintManager.AuthenticationCallback
    {
        private Context mainActivity;
        private int wrongcounts = 0;
        FingerprintManager fingerprintManager;
        
        public FingerprintHandler(Context mainActivity, FingerprintManager fingerprintManager)
        {
            this.mainActivity = mainActivity;
            this.fingerprintManager = fingerprintManager;

        }

        internal void StartAuthentication(FingerprintManager fingerprintManager, FingerprintManager.CryptoObject cryptoObject)
        {
            CancellationSignal cenCancellationSignal = new CancellationSignal();
            if (ActivityCompat.CheckSelfPermission(mainActivity, Manifest.Permission.UseFingerprint) != (int)Android.Content.PM.Permission.Granted)
                return;           
            fingerprintManager.Authenticate(cryptoObject, cenCancellationSignal, 0, this, null);
        }


        public override void OnAuthenticationFailed()
        {
            wrongcounts = wrongcounts+1;
            Toast.MakeText(mainActivity, "Fingerprint Authentication failed! "+Convert.ToString(3- wrongcounts)+" left", ToastLength.Short).Show();

            if(wrongcounts==3)
            {
                fingerprintManager.UnregisterFromRuntime();
               // mainActivity.StartActivity(new Intent(mainActivity, typeof(PasswordActivity)).SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask));
                  
            }

        }

        public override void OnAuthenticationSucceeded(FingerprintManager.AuthenticationResult result)
        {
            base.OnAuthenticationSucceeded(result);
            mainActivity.StartActivity(new Intent(mainActivity, typeof(MainAppScreen)).SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask));

        }
    }
}