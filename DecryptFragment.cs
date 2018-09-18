using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Android.OS;
using Android.Views;
using Android.Widget;
using static DataEncryptAndDecrypt.CommonMethods;

namespace DataEncryptAndDecrypt
{
    public sealed class DecryptFragment : Android.Support.V4.App.Fragment, IChangeViewvalues
    {

        View decryptView;
        //MainDecyptLayout
        EditText dlFileSelectTextBox;
        Spinner dlTypeofAccountSpinner;
        EditText dlEncryptionKeyTextBox;
        ListView decrypteddataListView;
        Button decryptDataButton;
        RadioGroup dedataTypeRadioGroup;
        RadioButton dedataTypeRadiobutton;
        int selectedId;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

      
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            decryptView= inflater.Inflate(Resource.Layout.MainDecryptLayout, container, false);
            
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
            dedataTypeRadioGroup= decryptView.FindViewById<RadioGroup>(Resource.Id.DlDataTypeRadioGroup);
            dedataTypeRadioGroup.CheckedChange += ChnageinDatatype;
            selectedId = dedataTypeRadioGroup.CheckedRadioButtonId;
            dedataTypeRadiobutton = (RadioButton)decryptView.FindViewById(selectedId);

            return decryptView;
        }

        private void ChnageinDatatype(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            selectedId = dedataTypeRadioGroup.CheckedRadioButtonId;
            dedataTypeRadiobutton = (RadioButton)decryptView.FindViewById(selectedId);
            if (dlFileSelectTextBox.Text.Length > 0)
            {
             Spinner(MFileData.Mydata, dlTypeofAccountSpinner, dedataTypeRadiobutton.Text);
            }
            else
            {
                MessageDialog(title: "Error", data: "Please Select File First", context: _context);
            }
        }

        public void Changevalues()
        {
            dlFileSelectTextBox.Text = Filepath;
            Spinner(MFileData.Mydata, dlTypeofAccountSpinner, dedataTypeRadiobutton.Text);          

        }

        private void DecryptButtonClick(object sender, EventArgs e)
        {
            List<System.String> datacol = new List<System.String>();

            if (dlFileSelectTextBox.Text.Length != 0 && dlEncryptionKeyTextBox.Text.Length != 0)
            {
                if (dedataTypeRadiobutton.Text == "CardInfo")
                {
                    var selectedCardInfo = MFileData.Mydata.Cardinfo.FindAll(x => x.Source == dlTypeofAccountSpinner.SelectedItem.ToString());

                    foreach (Cardinfo str in selectedCardInfo)
                    {
                        if (str.Source.Equals(dlTypeofAccountSpinner.SelectedItem.ToString()))
                        {                            
                            datacol.Add(str.Source);
                            datacol.Add("CardNo: " + DecryptPassword(str.CardNo, dlEncryptionKeyTextBox.Text));
                            datacol.Add("IfscCode: " + DecryptPassword(str.IFSCCODE, dlEncryptionKeyTextBox.Text));
                            datacol.Add("ValidFrom: " + DecryptPassword(str.ValidFrom, dlEncryptionKeyTextBox.Text));
                            datacol.Add("Validthrough: " + DecryptPassword(str.Validthrough, dlEncryptionKeyTextBox.Text));
                            datacol.Add("NameOnCard: " + DecryptPassword(str.NameOnCard, dlEncryptionKeyTextBox.Text));
                            datacol.Add("ThreeDSecureCode: " + DecryptPassword(str.ThreeDSecureCode, dlEncryptionKeyTextBox.Text));
                            datacol.Add("Cvv: " + DecryptPassword(str.CVV, dlEncryptionKeyTextBox.Text));
                            datacol.Add("Notes: " + DecryptPassword(str.Notes, dlEncryptionKeyTextBox.Text));

                            if (DecryptPassword(str.Notes, dlEncryptionKeyTextBox.Text).Length>30)
                            {
                                List<string> result = new List<string>(Regex.Split(DecryptPassword(str.Notes, dlEncryptionKeyTextBox.Text), pattern: @"(?<=\G.{30})", options: RegexOptions.Singleline));

                                foreach(string sstr in result)
                                {
                                    datacol.Add(sstr);
                                }
                            }

                            
                        }
                    }
                }
                else
                {
                    var selectedUnamepass = MFileData.Mydata.Unamepass.FindAll(x => x.Source == dlTypeofAccountSpinner.SelectedItem.ToString());

                    foreach (Unamepass str in selectedUnamepass)
                    {
                        //if (str.Source.Equals(dlTypeofAccountSpinner.SelectedItem.ToString()))
                        //{
                        datacol.Add(str.Source);
                        datacol.Add(DecryptPassword(str.UserName, dlEncryptionKeyTextBox.Text));
                        datacol.Add(DecryptPassword(str.Password, dlEncryptionKeyTextBox.Text));
                        // }

                    }
                }
                if (datacol.Count == 0)
                {
                    datacol.Add("No Data");
                }

                var adapter = new ArrayAdapter<String>(_context, Android.Resource.Layout.SimpleSpinnerItem, datacol);               
                decrypteddataListView.Adapter = adapter;
            }
            else
            {
                MessageDialog("Info", "Please Enter data in FileSelect/EncryptionKey fields", _context);
            }
        }
    }
}