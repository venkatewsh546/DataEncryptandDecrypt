using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android;
using static DataEncryptAndDecrypt.CommonMethods;
//using System.Web.Script.Serialization;

namespace DataEncryptAndDecrypt
{
    public class ChangekeyFragment : Android.Support.V4.App.Fragment, Classvalues
    {
        View changeKeyView;
        //MainChangeKeyLayout
        EditText ckFileSelectTextBox;
        EditText ckoldkey;
        EditText cknewkey;
        Button changeEncryptKeyButton;

        public void Changevalues()
        {
            ckFileSelectTextBox.Text = Filepath;
        
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            changeKeyView=(View)inflater.Inflate(Resource.Layout.MainChangeKeyLayout, container, false);

           

            ckFileSelectTextBox = changeKeyView.FindViewById<EditText>(Resource.Id.CkFileSelectEditBox);
            ckFileSelectTextBox.SetRawInputType(Android.Text.InputTypes.Null);
            ckFileSelectTextBox.SetCursorVisible(true);
            ckFileSelectTextBox.Click += CommonMethods.DisplayFilesAndFolders;

            ckoldkey = changeKeyView.FindViewById<EditText>(Resource.Id.Ckoldkey);
            cknewkey = changeKeyView.FindViewById<EditText>(Resource.Id.Cknewkey);

            changeEncryptKeyButton = changeKeyView.FindViewById<Button>(Resource.Id.ChangeKeyButton);
            changeEncryptKeyButton.Click += ChangeEncryptKeyButtonClick;

            return changeKeyView;
        }


        private void ChangeEncryptKeyButtonClick(object sender, EventArgs e)
        {
            string[] singlestr;

            FileData newkeyFileData = new FileData();
            newkeyFileData.mydata.cardinfo = CommonMethods.FileData.mydata.cardinfo;

            List<Unamepass> NewdataEncrypt = new List<Unamepass>();


            try
            {
                if (ckFileSelectTextBox.Text.Length > 0 &&
                    (!System.String.IsNullOrEmpty(cknewkey.Text) || !System.String.IsNullOrWhiteSpace(cknewkey.Text)) &&
                    (!System.String.IsNullOrEmpty(ckoldkey.Text) || !System.String.IsNullOrWhiteSpace(ckoldkey.Text))
                    )
                {
                   
                    foreach (Unamepass str in CommonMethods.FileData.mydata.unamepass)
                    {
                        NewdataEncrypt.Add(new Unamepass()
                        {
                            Source = str.Source,
                            UserName = EncryptPassword(DecryptPassword(str.UserName, ckoldkey.Text), cknewkey.Text),
                            Password = EncryptPassword(DecryptPassword(str.Password, ckoldkey.Text), cknewkey.Text)
                        });
                    }

                    newkeyFileData.mydata.unamepass = NewdataEncrypt;

                    //System.IO.File.WriteAllText(CommonMethods.Filepath.ToString(), new JavaScriptSerializer().Serialize(newkeyFileData));

                    CommonMethods.FileData = newkeyFileData;

                    CommonMethods.MessageDialog("Info", "Success" + System.Environment.NewLine + "Key Changed Successfully", Activity.ApplicationContext);
                }
                else
                {
                    CommonMethods.MessageDialog("Info", "Error" + System.Environment.NewLine + "Some Fileds are Empty", Activity.ApplicationContext);
                }
            }
            catch (Exception Ex)
            {
                MessageDialog("Error", "ChangeEncryptionKey \n" + Ex.Message, Activity.ApplicationContext);
            }

        }

       
    }
}