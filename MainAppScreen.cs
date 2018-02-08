using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using static DataEncryptAndDecrypt.CommonMethods;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.Support.V4.Widget;
using Android.Support.V7.App;

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
        private SupportFragment mCurrentFragment = new SupportFragment();

        EncryptFragment encryptFragment;
        DecryptFragment decryptFragment;
        DeleteFragment deleteFragment;
        ChangekeyFragment changekeyFragment;
        CardInfoFragment cardInfoFragment;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            Context = this;

            mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawerlayout);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.leftdrawer);
            mLeftDrawer.Tag = 0;
            SetSupportActionBar(mToolbar);
           
            mLeftDataSet = new List<string>
            {
                "Encrypt User Data",
                "Encrypt Card Data",
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
                openedResource: Resource.String.openDrawer,     //Opened Message
                closedResource: Resource.String.closeDrawer //Closed Message
            );

            mDrawerLayout.AddDrawerListener(mDrawerToggle);

            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            mDrawerToggle.SyncState();

            encryptFragment = new EncryptFragment();
            ft = SupportFragmentManager.BeginTransaction();
            ft.Add(Resource.Id.DynamicFragments, encryptFragment);
            ft.Commit();
            mCurrentFragment = encryptFragment;
            Fragmentobj = encryptFragment;

        }       
       
        private void MenuListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            switch (e.Id)
            {
                case 0:
                    encryptFragment = new EncryptFragment();
                    ft.Dispose();
                    ft = SupportFragmentManager.BeginTransaction();
                    ft.Detach(mCurrentFragment).Attach(mCurrentFragment);
                    ft.Replace(Resource.Id.DynamicFragments, encryptFragment);
                    ft.Commit();
                    mCurrentFragment = encryptFragment;
                    Fragmentobj = encryptFragment;
                    break;
                case 1:
                    cardInfoFragment = new CardInfoFragment();
                    ft.Dispose();
                    ft = SupportFragmentManager.BeginTransaction();
                    ft.Detach(mCurrentFragment).Attach(mCurrentFragment);
                    ft.Replace(Resource.Id.DynamicFragments, cardInfoFragment);
                    ft.Commit();
                    mCurrentFragment = cardInfoFragment;
                    Fragmentobj = cardInfoFragment;
                    break;
                case 2:
                    decryptFragment = new DecryptFragment();
                    ft.Dispose();
                    ft = SupportFragmentManager.BeginTransaction();
                    ft.Detach(mCurrentFragment).Attach(mCurrentFragment);
                    ft.Replace(Resource.Id.DynamicFragments, decryptFragment);
                    ft.Commit();
                    mCurrentFragment = decryptFragment;
                    Fragmentobj = decryptFragment;
                    break;
                case 3:
                    deleteFragment = new DeleteFragment();
                    ft.Dispose();
                    ft = SupportFragmentManager.BeginTransaction();
                    ft.Detach(mCurrentFragment).Attach(mCurrentFragment);
                    ft.Replace(Resource.Id.DynamicFragments, deleteFragment);
                    ft.Commit();
                    mCurrentFragment = deleteFragment;
                    Fragmentobj = deleteFragment;
                    break;
                case 4:
                    changekeyFragment = new ChangekeyFragment();
                    ft.Dispose();
                    ft = SupportFragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.DynamicFragments, changekeyFragment);
                    ft.Commit();
                    mCurrentFragment = changekeyFragment;
                    Fragmentobj = changekeyFragment;
                    break;
            }
            mDrawerLayout.CloseDrawers();
            mDrawerToggle.SyncState();
        }

        public override void OnBackPressed()
        {
            Process.KillProcess(Process.MyPid());
        }
    }
}

