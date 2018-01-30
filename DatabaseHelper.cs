using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Database.Sqlite;
using Android.Database;

namespace DataEncryptAndDecrypt
{
    public class DatabaseHelper : SQLiteOpenHelper
    {

        private static readonly int DATABASE_VERSION = 1;
        private static readonly String DATABASE_NAME = "UserManager.db";
        private static readonly String TABLE_USER = "user";
        private static readonly String COLUMN_USER_ID = "user_id";
        private static readonly String COLUMN_USER_PASSWORD = "user_password";

        private String CREATE_USER_TABLE = "CREATE TABLE IF NOT EXISTS " + TABLE_USER + "(" + COLUMN_USER_ID + " TEXT PRIMARY KEY ," + COLUMN_USER_PASSWORD + " TEXT" + ")";

        // drop table sql query
        private String DROP_USER_TABLE = "DROP TABLE IF EXISTS " + TABLE_USER;

        public DatabaseHelper(Context context, string name, SQLiteDatabase.ICursorFactory factory, int version) : base(context, name, factory, version)
        {
            name = DATABASE_NAME;
            factory = null;
            version = DATABASE_VERSION;
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(CREATE_USER_TABLE);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL(DROP_USER_TABLE);
            // Create tables again
            OnCreate(db);
        }


        public bool CheckForOldPassword()
        {
            SQLiteDatabase db = this.WritableDatabase;
            ICursor cursor = db.Query(TABLE_USER, new String[] { COLUMN_USER_ID, COLUMN_USER_PASSWORD }, null, null, null, null, null, null);

            if (cursor == null)
            {
                db.Close();
                return false;
            }
            else
            {
                db.Close();
                return true;
            }   
        }
        
        public string GetPassword()
        {

            SQLiteDatabase db = this.WritableDatabase;
            String Password = null;

            ICursor cursor = db.RawQuery("select user_password from user where user_id='Password'",null);

            while (cursor.MoveToFirst())
            {
                Password = cursor.GetString(0);
            }
            cursor.Close();
            db.Close();

            return Password;
        }

        public void addUser(String str)
        {
            SQLiteDatabase db = this.WritableDatabase;

            ContentValues values = new ContentValues();           
            values.Put(COLUMN_USER_PASSWORD, str);
            values.Put(COLUMN_USER_ID, "Password");

            ICursor cursor = db.Query(TABLE_USER, new String[] { COLUMN_USER_ID, COLUMN_USER_PASSWORD },null,null, null, null, null, null);
            if (cursor == null)
            {
                db.Insert(TABLE_USER, null, values);
            }
            db.Close();
        }

        public void updateUser(String oldpass,string newpass)
        {
            SQLiteDatabase db = this.WritableDatabase;

            ContentValues values = new ContentValues();
            values.Put(COLUMN_USER_PASSWORD, newpass);            

            // updating row
            db.Update(TABLE_USER, values, COLUMN_USER_ID + " =?",new String[] {"Password" });
            db.Close();
        }
    }
}