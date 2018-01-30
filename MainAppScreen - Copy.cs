#if false
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

        View encryptView;
        View deleteView;
        View changeKeyView;
        View decryptView;
      

        //MainEncyptLayout
        EditText elFileSelectTextBox;
        EditText elTypeOfAccountTextBox;
        EditText elUserNameTextBox;
        EditText elPasswordTextBox;
        EditText elEncryptionKeyTextBox;
        
        //MainDecyptLayout
        EditText dlFileSelectTextBox;
        Spinner dlTypeofAccountSpinner;
        EditText dlEncryptionKeyTextBox;
        ListView decrypteddataListView;

        //MainDeleteLayout
        EditText delFileSelectTextBox;
        EditText delEncryptionKeyTextBox;
        Spinner delTypeofAccountSpinner;
        Spinner delUserNameSpinner;
       

        //MainChangeKeyLayout
        EditText ckFileSelectTextBox;
        EditText ckoldkey;
        EditText cknewkey;
       

        Button encryptDataButton;
        Button decryptDataButton;
        Button changeEncryptKeyButton;
        Button deleteDataButton;

      
        static List<System.String> dataFromFile;       
        Android.App.AlertDialog.Builder builder;
        Dialog dialog;
        List<String> dirs;       
        static StringBuilder stringBuilder;
        LinearLayout.LayoutParams widthhight;

        static FileData fileData;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            widthhight = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

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
            

             encryptView = FindViewById<View>(Resource.Id.EncryptLayout);             
             decryptView = FindViewById<View>(Resource.Id.DecryptLayout);
             deleteView = FindViewById<View>(Resource.Id.DeleteLayout);
             changeKeyView = FindViewById<View>(Resource.Id.ChangeKeyLayout);
            
            //MainEncyptLayout
            elFileSelectTextBox = encryptView.FindViewById<EditText>(Resource.Id.ElFileSelectTextBox);
            elFileSelectTextBox.SetRawInputType(Android.Text.InputTypes.Null);
            elFileSelectTextBox.SetCursorVisible(true);
            elFileSelectTextBox.Click += DisplayFilesAndFolders;
           
            elTypeOfAccountTextBox = encryptView.FindViewById<EditText>(Resource.Id.ElTypeOfAccounTextBox);
            

            elUserNameTextBox = encryptView.FindViewById<EditText>(Resource.Id.ElUserNameTextBox);
            elPasswordTextBox = encryptView.FindViewById<EditText>(Resource.Id.ElPasswordTextBox);
            elEncryptionKeyTextBox = encryptView.FindViewById<EditText>(Resource.Id.ElEncryptionKeyTextBox);

            encryptDataButton = encryptView.FindViewById<Button>(Resource.Id.EncryptButton);
            encryptDataButton.Click += EncryptButtonClick;

            //MainDecyptLayout
            dlFileSelectTextBox = decryptView.FindViewById<EditText>(Resource.Id.DlFileSelectTextBox);
            dlFileSelectTextBox.SetRawInputType(Android.Text.InputTypes.Null);
            dlFileSelectTextBox.SetCursorVisible(true);
            dlFileSelectTextBox.Click += DisplayFilesAndFolders;

            dlTypeofAccountSpinner = decryptView.FindViewById<Spinner>(Resource.Id.DlTypeofAccountSpinner);

            dlEncryptionKeyTextBox = decryptView.FindViewById<EditText>(Resource.Id.DlEncryptionKeyTextBox);  
            
             decrypteddataListView = decryptView.FindViewById<ListView>(Resource.Id.DecrypteddataListView);
            decrypteddataListView.ItemLongClick += ListViewLongClickListener;

            decryptDataButton = decryptView.FindViewById<Button>(Resource.Id.DecryptButton);
            decryptDataButton.Click += DecryptButtonClick;


            //MainDeleteLayout
            delFileSelectTextBox =deleteView.FindViewById<EditText>(Resource.Id.DelFileSelectTextBox);
            delFileSelectTextBox.SetRawInputType(Android.Text.InputTypes.Null);
            delFileSelectTextBox.SetCursorVisible(true);
            delFileSelectTextBox.Click += DisplayFilesAndFolders;
            delFileSelectTextBox.NextFocusDownId = Resource.Id.DelEncryptionKeyTextBox;


            delEncryptionKeyTextBox = deleteView.FindViewById<EditText>(Resource.Id.DelEncryptionKeyTextBox);

            delTypeofAccountSpinner = deleteView.FindViewById<Spinner>(Resource.Id.DelTypeofAccountSpinner);
            delTypeofAccountSpinner.ItemSelected += DelTypeOfAcoountSpinnerItemClick;
            delUserNameSpinner = deleteView.FindViewById<Spinner>(Resource.Id.DelUserNameSpinner);

            deleteDataButton = deleteView.FindViewById<Button>(Resource.Id.DeleteDataButton);
            deleteDataButton.Click += DeleteButtonClick;

            //MainChangeKeyLayout
            ckFileSelectTextBox = changeKeyView.FindViewById<EditText>(Resource.Id.CkFileSelectEditBox);
            ckFileSelectTextBox.SetRawInputType(Android.Text.InputTypes.Null);
            ckFileSelectTextBox.SetCursorVisible(true);
            ckFileSelectTextBox.Click += DisplayFilesAndFolders;

            ckoldkey = changeKeyView.FindViewById<EditText>(Resource.Id.Ckoldkey);
            cknewkey = changeKeyView.FindViewById<EditText>(Resource.Id.Cknewkey);

            changeEncryptKeyButton = changeKeyView.FindViewById<Button>(Resource.Id.ChangeKeyButton);
            changeEncryptKeyButton.Click += ChangeEncryptKeyButtonClick;

            dirs = new List<String>();

            decryptView.Visibility = ViewStates.Gone;
            deleteView.Visibility = ViewStates.Gone;
            changeKeyView.Visibility = ViewStates.Gone;
     
        }       
       
        private void MenuListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            switch (e.Id)
            {
                case 0:
                    encryptitemselect();
                    break;
                case 1:
                    decryptitemselect();
                    break;
                case 2:
                    deleteitemselect();
                    break;
                case 3:
                    changekeyitemselect();
                    break;
            }
            mDrawerLayout.CloseDrawers();
            mDrawerToggle.SyncState();
        }

        private void encryptitemselect()
        {
            encryptView.Visibility = ViewStates.Visible;
            encryptView.ClearAnimation();
            decryptView.Visibility = ViewStates.Gone;
            deleteView.Visibility = ViewStates.Gone;
            changeKeyView.Visibility = ViewStates.Gone;           
        }

        private void decryptitemselect()
        {
            encryptView.Visibility = ViewStates.Gone;
            decryptView.Visibility = ViewStates.Visible;
            deleteView.Visibility = ViewStates.Gone;
            changeKeyView.Visibility = ViewStates.Gone;
        }

        private void deleteitemselect()
        {
            encryptView.Visibility = ViewStates.Gone;
            decryptView.Visibility = ViewStates.Gone;
            deleteView.Visibility = ViewStates.Visible;
            changeKeyView.Visibility = ViewStates.Gone;
        }

        private void changekeyitemselect()
        {
            encryptView.Visibility = ViewStates.Gone;
            decryptView.Visibility = ViewStates.Gone;
            deleteView.Visibility = ViewStates.Gone;
            changeKeyView.Visibility = ViewStates.Visible;
         }      

        private void EncryptButtonClick(object sender, EventArgs e)
        {
            EncryptbuttonWriteToFile();
            elTypeOfAccountTextBox.Text = String.Empty;
            elUserNameTextBox.Text = String.Empty;
            elPasswordTextBox.Text = String.Empty;
        }

        private void DecryptButtonClick(Object sender, System.EventArgs e)
        {
            List<System.String> datacol = new List<System.String>();

            if (dlFileSelectTextBox.Text.Length != 0 && dlEncryptionKeyTextBox.Text.Length != 0)
            {
              foreach (string str in dataFromFile)
              {
                    if (str.Split(',')[0].Equals(dlTypeofAccountSpinner.SelectedItem.ToString()))
                    {                      
                        datacol.Add(str.Split(',')[0]);
                        datacol.Add(DecryptPassword(str.Split(',')[1], dlEncryptionKeyTextBox.Text));
                        datacol.Add(DecryptPassword(str.Split(',')[2], dlEncryptionKeyTextBox.Text));
                    }
              }
                var adapter = new ArrayAdapter<System.String>(this, Android.Resource.Layout.SimpleSpinnerItem, datacol);
                decrypteddataListView.Adapter = adapter;
            }
            else
            {
                MessageDialog("Info", "Please Enter data in FileSelect/EncryptionKey fields", this);
            }
        }

        private void DeleteButtonClick(object sender, EventArgs e)
        {
            try
            {
                if ((!System.String.IsNullOrEmpty(delTypeofAccountSpinner.SelectedItem.ToString()) || !System.String.IsNullOrWhiteSpace(delTypeofAccountSpinner.SelectedItem.ToString())) &&
                   (!System.String.IsNullOrEmpty(delUserNameSpinner.SelectedItem.ToString()) || !System.String.IsNullOrWhiteSpace(delUserNameSpinner.SelectedItem.ToString())) &&
                   (!System.String.IsNullOrEmpty(delFileSelectTextBox.Text) || !System.String.IsNullOrWhiteSpace(delFileSelectTextBox.Text)) &&
                   (!System.String.IsNullOrEmpty(delEncryptionKeyTextBox.Text) || !System.String.IsNullOrWhiteSpace(delEncryptionKeyTextBox.Text))
                   )
                {
                                  
                    var index = dataFromFile.FindIndex(x => x.StartsWith(delTypeofAccountSpinner.SelectedItem.ToString().ToUpper() + "," + EncryptPassword(delUserNameSpinner.SelectedItem.ToString(), delEncryptionKeyTextBox.Text)));

                    dataFromFile.RemoveAt(index);

                    System.IO.File.WriteAllLines(delFileSelectTextBox.Text, dataFromFile.ToArray());
                    Spinner(dataFromFile);
                    Toast.MakeText(this, "data Deleted successfully", ToastLength.Short).Show();
                }
                else
                {
                    MessageDialog("Error", "enter data in all Required Fields \n TypeOfAccount \n  UserName \n FileSelect \n EncryptionKey", this);
                }
            }
            catch (Exception Ex)
            {
                MessageDialog("Error", "ChangeEncryptionKey \n" + Ex.Message, this);
            }
        }

        private void ChangeEncryptKeyButtonClick(object sender, EventArgs e)
        {
            string[] singlestr;
            try
            {
                if (ckFileSelectTextBox.Text.Length > 0 &&
                    (!System.String.IsNullOrEmpty(cknewkey.Text) || !System.String.IsNullOrWhiteSpace(cknewkey.Text)) &&
                    (!System.String.IsNullOrEmpty(ckoldkey.Text) || !System.String.IsNullOrWhiteSpace(ckoldkey.Text))
                    )
                {                   
                    List<String> NewdataEncrypt = new List<string>();
                    NewdataEncrypt.Insert(0, "Source,UserName,Password");

                    foreach (String str in dataFromFile)
                    {
                        NewdataEncrypt.Add(str.Split(',')[0] + "," +
                        EncryptPassword(DecryptPassword(str.Split(',')[1], ckoldkey.Text), cknewkey.Text) + "," +
                        EncryptPassword(DecryptPassword(str.Split(',')[2], ckoldkey.Text), cknewkey.Text));
                    }
                    
                    System.IO.File.WriteAllLines(ckFileSelectTextBox.Text, NewdataEncrypt.ToArray());

                    dataFromFile = new List<string>(NewdataEncrypt);

                    CommonMethods.MessageDialog("Info", "Success" + System.Environment.NewLine + "Key Changed Successfully", this);
                }
                else
                {
                    CommonMethods.MessageDialog("Info", "Error" + System.Environment.NewLine + "Some Fileds are Empty", this);
                }
            }
            catch (Exception Ex)
            {
                MessageDialog("Error", "ChangeEncryptionKey \n" + Ex.Message, this);
            }

        }
                
        private void DelTypeOfAcoountSpinnerItemClick(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (delEncryptionKeyTextBox.Text.Length == 0 || delFileSelectTextBox.Text.Length == 0
                 && !delTypeofAccountSpinner.SelectedItem.ToString().Equals("<<<< Select Item >>>>"))
            {
                MessageDialog("Info", "select File/enter Encryption key Before Selecting Item", this);

            }
            else if (delEncryptionKeyTextBox.Text.Length > 0 || delFileSelectTextBox.Text.Length > 0
                 && !delTypeofAccountSpinner.SelectedItem.ToString().Equals("<<<< Select Item >>>>"))
            {
                UserNameSpinner(dataFromFile, ((Spinner)sender).GetItemAtPosition(e.Position).ToString());
            }

        }
         
        public override void OnBackPressed()
        {
               Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }       

        private void ListViewLongClickListener(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            string selectedFromList = ((ListView)e.Parent).GetItemAtPosition(e.Position).ToString();
            ClipboardManager clipboard = (ClipboardManager)GetSystemService(ClipboardService);
            ClipData clip = ClipData.NewPlainText(selectedFromList, selectedFromList);
            clipboard.PrimaryClip = clip;
            Toast.MakeText(this, text: "Copied To Clipboard", duration: ToastLength.Long).Show();
        }
                
        private void DisplayFilesAndFolders(object sender, EventArgs e)
        {
            try
            {
                builder = new Android.App.AlertDialog.Builder(this);
                dirs.Clear();
                Java.IO.File dirFile;

                dirFile = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);
                dirs.Add("..");

                foreach (Java.IO.File file in dirFile.ListFiles())
                {
                    if (file.IsDirectory)
                    {
                        dirs.Add(file.Name + "/");
                    }
                    else
                    {
                        dirs.Add(file.Name);
                    }
                }

                using (var adapter = new ArrayAdapter<System.String>(this, Android.Resource.Layout.SimpleListItemSingleChoice, dirs))
                {
                    ListView dirtext = new ListView(this)
                    {
                        Adapter = adapter
                    };
                    dirtext.ItemClick += OnContextItemSelected;
                    builder.SetView(dirtext);
                    builder.SetTitle("folders");
                    dialog = builder.Create();
                    dialog.Show();
                };
            }

            catch (Exception ex)
            {
               MessageDialog("Error msg :-", "displayfilesandfolders method :-" + ex.Message,this);
            }
        }
     
        private void OnContextItemSelected(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                dirs.Clear();
                String str = e.Parent.GetItemAtPosition(e.Position).ToString();

                if (str.StartsWith(".."))
                {
                    DisplayFilesAndFolders(sender,e);
                    return;
                }
                else
                {
                    Java.IO.File dirFile = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/" + str);
                    dirs.Add("..");
                    dirs.Add(str);                  

                    if (dirFile.IsDirectory)
                    {
                        foreach (Java.IO.File file in dirFile.ListFiles())
                        {

                            if (file.IsDirectory)
                            {
                                dirs.Add(str + file.Name + "/");
                            }
                            else
                            {
                                dirs.Add(str + file.Name);
                            }

                        }
                    }
                    else if (dirFile.IsFile)
                    {
                        dataFromFile = new List<String>(GetDataFromFile(dirFile.AbsolutePath));

                        if (encryptView.Visibility == ViewStates.Visible)
                        {
                            elFileSelectTextBox.Text = dirFile.AbsolutePath;
                        }
                        else if(decryptView.Visibility == ViewStates.Visible)
                        {
                            dlFileSelectTextBox.Text = dirFile.AbsolutePath;                            
                            Spinner(dataFromFile);                                                       
                        }
                        else if (deleteView.Visibility == ViewStates.Visible)
                        {
                            delFileSelectTextBox.Text = dirFile.AbsolutePath;
                            Spinner(dataFromFile);
                        }
                        else if (changeKeyView.Visibility == ViewStates.Visible)
                        {
                            ckFileSelectTextBox.Text = dirFile.AbsolutePath;
                        }                       

                        dialog.Dismiss();
                        return;
                    }
                }
                dialog.Dismiss();
                builder.Dispose();
                builder = new Android.App.AlertDialog.Builder(this);
                builder.SetTitle("Selected Content");

                var adapter = new ArrayAdapter<System.String>(this, Android.Resource.Layout.SimpleSelectableListItem, dirs);
                ListView dirtext = new ListView(this)
                {
                    Adapter = adapter,

                };

                dirtext.LayoutParameters = new LayoutParams(ListView.LayoutParams.MatchParent, ListView.LayoutParams.MatchParent);
                dirtext.ItemClick += OnContextItemSelected;
                builder.SetView(dirtext);
                dialog = builder.Create();
                dialog.Show();
            }

            catch (Exception ex)
            {
               MessageDialog("Error:", "OnContextItemSelected" + ex.Message,this);
            }

        }
        
        private void EncryptbuttonWriteToFile()
        {
            System.String account = string.Empty;
            System.String userName = string.Empty;
            System.String password = string.Empty;
            System.String encryptKey = string.Empty;
            try
            {
                Java.IO.File myFile;
                BufferedWriter writer;
                
                if (System.String.IsNullOrEmpty(elFileSelectTextBox.Text))
                {
                    myFile = new Java.IO.File(GetExternalFilesDir(null), "EncryptDecrypt.txt");
                    elFileSelectTextBox.Text = myFile.AbsolutePath;
                }
                else
                {
                    myFile = new Java.IO.File(elFileSelectTextBox.Text);
                }               


                if ((!System.String.IsNullOrEmpty(elTypeOfAccountTextBox.Text) || !System.String.IsNullOrWhiteSpace(elTypeOfAccountTextBox.Text)) &&
                    (!System.String.IsNullOrEmpty(elUserNameTextBox.Text) || !System.String.IsNullOrWhiteSpace(elUserNameTextBox.Text)) &&
                    (!System.String.IsNullOrEmpty(elPasswordTextBox.Text) || !System.String.IsNullOrWhiteSpace(elPasswordTextBox.Text)) &&
                    (!System.String.IsNullOrEmpty(elEncryptionKeyTextBox.Text) || !System.String.IsNullOrWhiteSpace(elEncryptionKeyTextBox.Text)))
                {
                    account = elTypeOfAccountTextBox.Text;
                    userName = EncryptPassword(elUserNameTextBox.Text, elEncryptionKeyTextBox.Text);
                    password = EncryptPassword(elPasswordTextBox.Text, elEncryptionKeyTextBox.Text);
                    encryptKey = elEncryptionKeyTextBox.Text;

                    stringBuilder = new StringBuilder();
                    stringBuilder.AppendFormat("{0},{1},{2}",account.ToUpper(), userName, password);                   

                    if (!myFile.Exists())
                    {
                        myFile.CreateNewFile();
                        writer = new BufferedWriter(new FileWriter(myFile, true));
                        writer.Append("Source,UserName,Password");
                        writer.Append(stringBuilder.ToString());
                        writer.Close();
                        Toast.MakeText(this, "data encrypted successfully", ToastLength.Short).Show();
                    }
                    else
                    {
                        var index = dataFromFile.FindIndex(x => x.StartsWith(account.ToString().ToUpper() + "," + userName));

                        if (index != -1)
                        {
                            MessageDialog("Updaing", "Old Password Will Replace With New One",this);
                                                       
                            var searchdata = account.ToString().ToUpper() + "," +userName;

                            List<String> NewDataFromFile = new List<string>(dataFromFile);

                            foreach (String str in dataFromFile)
                            {
                                if (str.StartsWith(searchdata) || str == "")
                                {
                                    NewDataFromFile.Remove(str);
                                }
                            }

                            NewDataFromFile.Insert(0, "Source,UserName,Password");
                            NewDataFromFile.Add(stringBuilder.ToString());
                            System.IO.File.WriteAllLines(myFile.AbsolutePath, NewDataFromFile.ToArray());                           
                            dataFromFile = new List<String>(NewDataFromFile);
                            Toast.MakeText(this, "data updated successfully", ToastLength.Short).Show();
                        }
                        else
                        {
                            writer = new BufferedWriter(new FileWriter(myFile, true /*append*/));
                            writer.Append(System.Environment.NewLine+stringBuilder.ToString());
                            writer.Close();
                            Toast.MakeText(this, "data encrypted successfully", ToastLength.Short).Show();

                        }
                    }
                }
                else
                {
                    MessageDialog("info", "please enter data in all fileds",this);
                }
            }
            catch (System.IO.IOException e)
            {
                MessageDialog("Error....", "writeToFile :- " + e.Message,this);
            }
        }

        private void Spinner(List<System.String> listdata)
        {
            List<String> srclist = new List<String>();
            srclist.Add("<<<<Select Item>>>>");
            try
            {             
                foreach (string str in listdata)
                {
                    if (!srclist.Contains(str.Split(',')[0]))
                    {
                        srclist.Add(str.Split(',')[0]);                          
                    }
                }

                var srcTypeAdapter = new ArrayAdapter<System.String>(this, Android.Resource.Layout.SimpleSpinnerItem, srclist);
                srcTypeAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                                  
                if (decryptView.Visibility == ViewStates.Visible)
                {
                    dlTypeofAccountSpinner.Adapter = srcTypeAdapter;                  

                }
                else if (deleteView.Visibility == ViewStates.Visible)
                {
                    delTypeofAccountSpinner.Adapter = srcTypeAdapter;
                } 

                srclist.Clear();

            }
            catch (Exception ex)
            {
                MessageDialog("TypeOfAccountspinner", ex.Message,this);
            }
        }

        private void UserNameSpinner(List<string> dataFromFile, String typeofaccount)
        {           
            var userNameList = new List<String>();
            try
            {
                foreach (string str in dataFromFile)
                {
                    if (str.ToUpper().StartsWith(typeofaccount))
                    {
                        if (!userNameList.Contains(DecryptPassword(str.Split(',')[1],delEncryptionKeyTextBox.Text)))
                        {
                            userNameList.Add(DecryptPassword(str.Split(',')[1], delEncryptionKeyTextBox.Text));
                        }
                    }
                }

                var userNameAdapter = new ArrayAdapter<System.String>(this, Android.Resource.Layout.SimpleSpinnerItem, userNameList);
                userNameAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

                if (deleteView.Visibility == ViewStates.Visible)
                {
                    delUserNameSpinner.Adapter = userNameAdapter;
                }
                userNameList.Clear();
            }
            catch (Exception ex)
            {
                MessageDialog("UserNamespinner", ex.Message, this);
            }
        }

    }
}

#endif