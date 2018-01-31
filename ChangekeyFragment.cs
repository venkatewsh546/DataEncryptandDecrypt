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
using Newtonsoft.Json;
//using System.Web.Script.Serialization;

namespace DataEncryptAndDecrypt
{
    public sealed class ChangekeyFragment : Android.Support.V4.App.Fragment, IChangeViewvalues
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
            ckFileSelectTextBox.Click += DisplayFilesAndFolders;

            ckoldkey = changeKeyView.FindViewById<EditText>(Resource.Id.Ckoldkey);
            cknewkey = changeKeyView.FindViewById<EditText>(Resource.Id.Cknewkey);

            changeEncryptKeyButton = changeKeyView.FindViewById<Button>(Resource.Id.ChangeKeyButton);
            changeEncryptKeyButton.Click += ChangeEncryptKeyButtonClick;

            return changeKeyView;
        }


        private void ChangeEncryptKeyButtonClick(object sender, EventArgs e)
        {
            try
            {
                FileData nFileData = CommonMethods.MFileData;

                if (ckFileSelectTextBox.Text.Length > 0 &&
                    (!System.String.IsNullOrEmpty(cknewkey.Text) || !System.String.IsNullOrWhiteSpace(cknewkey.Text)) &&
                    (!System.String.IsNullOrEmpty(ckoldkey.Text) || !System.String.IsNullOrWhiteSpace(ckoldkey.Text))
                    )
                {
                    Unamepass unamepassdata;

                    for (int index = 0; index <= nFileData.mydata.unamepass.Count-1; index++)
                    {
                        unamepassdata = nFileData.mydata.unamepass[index];
                        nFileData.mydata.unamepass[index].UserName = EncryptPassword(DecryptPassword(unamepassdata.UserName, ckoldkey.Text), cknewkey.Text);
                        nFileData.mydata.unamepass[index].Password = EncryptPassword(DecryptPassword(unamepassdata.Password, ckoldkey.Text), cknewkey.Text);
                    }


                    //foreach (Unamepass str in _fileData.mydata.unamepass)
                    //{
                    //    newkeyFileData.mydata.unamepass.Add(new Unamepass()
                    //    {
                    //        Source = str.Source,
                    //        UserName = EncryptPassword(DecryptPassword(str.UserName, ckoldkey.Text), cknewkey.Text),
                    //        Password = EncryptPassword(DecryptPassword(str.Password, ckoldkey.Text), cknewkey.Text)
                    //    });
                    //}


                //foreach(Cardinfo ci in _fileData.mydata.cardinfo)
                //{
                //    newkeyFileData.mydata.cardinfo.Add(new Cardinfo()
                //    {
                //    Source = ci.Source,                            
                //    CardNo=EncryptPassword(DecryptPassword(ci.CardNo, ckoldkey.Text), cknewkey.Text),
                //    IFSCCODE = EncryptPassword(DecryptPassword(ci.IFSCCODE, ckoldkey.Text), cknewkey.Text),
                //    Validthrough = EncryptPassword(DecryptPassword(ci.Validthrough, ckoldkey.Text), cknewkey.Text),
                //    ValidFrom = EncryptPassword(DecryptPassword(ci.ValidFrom, ckoldkey.Text), cknewkey.Text),
                //    NameOnCard = EncryptPassword(DecryptPassword(ci.NameOnCard, ckoldkey.Text), cknewkey.Text),
                //    ThreeDSecureCode = EncryptPassword(DecryptPassword(ci.ThreeDSecureCode, ckoldkey.Text), cknewkey.Text),
                //    CVV = EncryptPassword(DecryptPassword(ci.CVV, ckoldkey.Text), cknewkey.Text),
                //    Notes = EncryptPassword(DecryptPassword(ci.Notes, ckoldkey.Text), cknewkey.Text)
                //    });
                //}


                System.IO.File.WriteAllText(CommonMethods.Filepath.ToString(), JsonConvert.SerializeObject(nFileData));
                CommonMethods.MessageDialog("Info", "Success" + System.Environment.NewLine + "Key Changed Successfully", _context);
                }
                else
                {
                    CommonMethods.MessageDialog("Info", "Error" + System.Environment.NewLine + "Some Fileds are Empty", _context);
                }
            }
            catch (Exception Ex)
            {
                MessageDialog("Error", "ChangeEncryptionKey \n" + Ex.Message, _context);
            }

        }

       
    }
}