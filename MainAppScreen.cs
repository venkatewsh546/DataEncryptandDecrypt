using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;
using Android.Content;
using Java.IO;
using System.Security.Cryptography;
using System.IO;
using Android.Media;
using System.Runtime.Remoting.Contexts;
using ViewGroup=Android.Views;
using Java.Net;
using static Android.App.ActionBar;
using static DataEncryptAndDecrypt.CommonMethods;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using System.Text;
using Newtonsoft.Json;

namespace DataEncryptAndDecrypt
{
    [Activity(Label = "DataEncryptAndDecrypt", MainLauncher = true, Theme = "@style/AppTheme")]
    public class MainAppScreen : AppCompatActivity
    {

        private SupportToolbar mToolbar;
        private MyActionBarDrawerToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        private ListView mLeftDrawer;
        private ArrayAdapter mLeftAdapter;
        private List<string> mLeftDataSet;
        Android.Support.V4.App.FragmentTransaction ft;   
        static FileData fileData;
        private SupportFragment mCurrentFragment = new SupportFragment();

        EncryptFragment encryptFragment;
        DecryptFragment decryptFragment;
        DeleteFragment deleteFragment;
        ChangekeyFragment changekeyFragment;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            CommonMethods.Context = this;
            encryptFragment = new EncryptFragment();
            decryptFragment = new DecryptFragment();
            deleteFragment = new DeleteFragment();
            changekeyFragment = new ChangekeyFragment();

            mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawerlayout);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.leftdrawer);
            mLeftDrawer.Tag = 0;
            SetSupportActionBar(mToolbar);

           
            mLeftDataSet = new List<string>
            {
                "Encrypt Data",
                "Decrypt Data",
                "Delete Data",
                "Change key"               
            };

            mLeftAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, mLeftDataSet);
            mLeftDrawer.Adapter = mLeftAdapter;
            mLeftDrawer.ItemClick += MenuListView_ItemClick;

            mDrawerToggle = new MyActionBarDrawerToggle(
                this,                           //Host Activity
                mDrawerLayout,                  //DrawerLayout
                Resource.String.openDrawer,     //Opened Message
                Resource.String.closeDrawer //Closed Message
            );

            mDrawerLayout.AddDrawerListener(mDrawerToggle);

            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            mDrawerToggle.SyncState();

            ft = SupportFragmentManager.BeginTransaction();
            ft.Add(Resource.Id.DynamicFragments, encryptFragment);
            ft.Commit();
            mCurrentFragment = encryptFragment;
            CommonMethods.Fragmentobj = encryptFragment;

        }       
       
        private void MenuListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            switch (e.Id)
            {
                case 0:
                    ft.Dispose();
                    ft = SupportFragmentManager.BeginTransaction();
                    ft.Detach(mCurrentFragment);
                    ft.Add(Resource.Id.DynamicFragments, encryptFragment);
                    ft.Commit();
                    mCurrentFragment = encryptFragment;
                    CommonMethods.Fragmentobj = encryptFragment;
                    break;
                case 1:
                    ft.Dispose();
                    ft = SupportFragmentManager.BeginTransaction();
                    ft.Detach(mCurrentFragment);
                    ft.Add(Resource.Id.DynamicFragments, decryptFragment);
                    ft.Commit();
                    mCurrentFragment = decryptFragment;
                    CommonMethods.Fragmentobj = decryptFragment;
                    break;
                case 2:
                    ft.Dispose();
                    ft = SupportFragmentManager.BeginTransaction();
                    ft.Detach(mCurrentFragment);
                    ft.Add(Resource.Id.DynamicFragments, deleteFragment);
                    ft.Commit();
                    mCurrentFragment = deleteFragment;
                    CommonMethods.Fragmentobj = deleteFragment;
                    break;
                case 3:
                    ft.Dispose();
                    ft = SupportFragmentManager.BeginTransaction();
                    ft.Detach(mCurrentFragment);
                    ft.Add(Resource.Id.DynamicFragments, changekeyFragment);
                    ft.Commit();
                    mCurrentFragment = changekeyFragment;
                    CommonMethods.Fragmentobj = changekeyFragment;
                    break;
            }
            mDrawerLayout.CloseDrawers();
            mDrawerToggle.SyncState();
        }
                
        public override void OnBackPressed()
        {
               Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }             
      
    }
}

