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
        RadioGroup ckdataTypeRadioGroup;
        RadioButton ckdataTypeRadiobutton;
        int selectedId;

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

            ckdataTypeRadioGroup = changeKeyView.FindViewById<RadioGroup>(Resource.Id.CkDataTypeRadioGroup);
            ckdataTypeRadioGroup.CheckedChange += ChnageinDatatype;
            selectedId = ckdataTypeRadioGroup.CheckedRadioButtonId;
            ckdataTypeRadiobutton = (RadioButton)changeKeyView.FindViewById(selectedId);

            return changeKeyView;
        }

        private void ChnageinDatatype(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            selectedId = ckdataTypeRadioGroup.CheckedRadioButtonId;
            ckdataTypeRadiobutton = (RadioButton)changeKeyView.FindViewById(selectedId);
        }

        private void ChangeEncryptKeyButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (ckFileSelectTextBox.Text.Length > 0 &&
                    (!System.String.IsNullOrEmpty(cknewkey.Text) || !System.String.IsNullOrWhiteSpace(cknewkey.Text)) &&
                    (!System.String.IsNullOrEmpty(ckoldkey.Text) || !System.String.IsNullOrWhiteSpace(ckoldkey.Text))
                    )
                {
                    if (ckdataTypeRadiobutton.Text == "CardInfo")
                    {
                        Cardinfo cardinfo;
                        for (int index = 0; index <= MFileData.Mydata.Cardinfo.Count - 1; index++)
                        {
                            cardinfo= MFileData.Mydata.Cardinfo[index];
                            MFileData.Mydata.Cardinfo[index].Source             = cardinfo.Source;
                            MFileData.Mydata.Cardinfo[index].CardNo             = EncryptPassword(DecryptPassword(cardinfo.CardNo, ckoldkey.Text), cknewkey.Text);
                            MFileData.Mydata.Cardinfo[index].IFSCCODE           = EncryptPassword(DecryptPassword(cardinfo.IFSCCODE, ckoldkey.Text), cknewkey.Text);
                            MFileData.Mydata.Cardinfo[index].Validthrough       = EncryptPassword(DecryptPassword(cardinfo.Validthrough, ckoldkey.Text), cknewkey.Text);
                            MFileData.Mydata.Cardinfo[index].ValidFrom          = EncryptPassword(DecryptPassword(cardinfo.ValidFrom, ckoldkey.Text), cknewkey.Text);
                            MFileData.Mydata.Cardinfo[index].NameOnCard         = EncryptPassword(DecryptPassword(cardinfo.NameOnCard, ckoldkey.Text), cknewkey.Text);
                            MFileData.Mydata.Cardinfo[index].ThreeDSecureCode   = EncryptPassword(DecryptPassword(cardinfo.ThreeDSecureCode, ckoldkey.Text), cknewkey.Text);
                            MFileData.Mydata.Cardinfo[index].CVV                = EncryptPassword(DecryptPassword(cardinfo.CVV, ckoldkey.Text), cknewkey.Text);
                            MFileData.Mydata.Cardinfo[index].Notes              = EncryptPassword(DecryptPassword(cardinfo.Notes, ckoldkey.Text), cknewkey.Text);
                        
                        }
                    }
                    else
                    {
                        Unamepass unamepassdata;
                        for (int index = 0; index <= MFileData.Mydata.Unamepass.Count - 1; index++)
                        {
                            unamepassdata = MFileData.Mydata.Unamepass[index];
                            MFileData.Mydata.Unamepass[index].UserName = EncryptPassword(DecryptPassword(unamepassdata.UserName, ckoldkey.Text), cknewkey.Text);
                            MFileData.Mydata.Unamepass[index].Password = EncryptPassword(DecryptPassword(unamepassdata.Password, ckoldkey.Text), cknewkey.Text);
                        }

                    }                    

                System.IO.File.WriteAllText(Filepath.ToString(), JsonConvert.SerializeObject(MFileData));
                    MessageDialog("Info", "Success" + System.Environment.NewLine + "Key Changed Successfully", _context);
                }
                else
                {
                    MessageDialog("Info", "Error" + System.Environment.NewLine + "Some Fileds are Empty", _context);
                }
            }
            catch (Exception Ex)
            {
                MessageDialog("Error", "ChangeEncryptionKey \n" + Ex.Message, _context);
            }

        }

       
    }
}