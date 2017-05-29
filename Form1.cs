// Decompiled with JetBrains decompiler
// Type: parser.Form1
// Assembly: ati, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C0AC51D4-230C-4F75-815E-A150F0ABAA1F
// Assembly location: C:\Users\javagraf\Desktop\Новая папка\Парсер_Галина_15.03\ati.exe

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml.Serialization;
using Xilium.CefGlue;
using Xilium.CefGlue.WindowsForms;

namespace parser {
    public class Form1 : Form {
        private string log_time = DateTime.Now.ToString("dd.MM.yyyy"); //текущее время в строку
                                                                       //private IContainer components = (IContainer) null; //контейнеры содержать в себе компоненты
        private bool IsMonitor = true; //?
        private MenuStrip menuStrip1; //окошко под менюшки
        private ToolStripMenuItem запуститьToolStripMenuItem;
        private ToolStripMenuItem остановитьToolStripMenuItem;
        private TabControl tabControl; //окошко для браузера
        private TextBox textBox_city; //город
        private Label label4;
        private Label label1; //балл
        private TextBox textBox_date_from;
        private Label label2; //дата с
        private TextBox textBox_date_to;
        private ComboBox comboBox_ball; //поле со списком балл
        private Label label6; //по
        private TextBox textBox_log;
        private TextBox textBox_login;
        private Label label5; //логин
        private TextBox textBox_pass;
        private Label label7; //пароль
        private Label label8; //документы
        private TextBox textBox_docs;
        private Label label9; //количество рекомендаций
        private TextBox textBox_recomend;
        private TextBox textBox_contact;
        private Label label10; //количество контактов
        private ComboBox comboBox_profile; //профиль
        private Label label11; //профиль 
        private CheckBox checkBox_blaclist1;
        private CheckBox checkBox_blaclist2;
        private CheckBox checkBox_blaclist3;
        private CheckBox checkBox_onlyIP;
        private CheckBox checkBox_SearchDeleted;
        private ComboBox comboBox_NoActivity; //нулевая активность
        private CheckBox checkBox_IsNoActivity; //чекбокс нулевой активности
        private CefFrame webControl; //предоставляет фрейм в окне браузера
        private CefFrame webControl_info; //предоставляет фрейм в окне браузера(info)
        private CefFrame webControl_bl; ////предоставляет фрейм в окне браузера (bl)
        private MyCefStringVisitor client; //для асинхроного получения строковых значений
        private Label label12;
        private ComboBox combo_box_step;
        private Thread thread_main;

        public DateTime convertDF;
        //public DateTime convertDa_to;
        public DateTime convertTbText_date_from;
        public DateTime readyConvertTbText_date_from;
        public DateTime dayFrom; //просто создал для того чтобы сохранять результать метода, использую ее для потребностей в коде
        private DateTime dayTo;  //просто создал для того чтобы сохранять результать метода, использую ее для потребностей в коде
        private string stepText;

        private bool testFlag = false;
        private bool dontJumpPlsMoreStep;
        private bool without_step;

        public string path;
        private TextBox textBoxCitys;
        private Label city_label;
        public double stepDouble = 0;

        private bool flag3;//если тру то не найдены записи на авто траст инфо, фалс - найдены

        private bool makeDate_from_one_time;
        private string date_from;

        private bool createOneTimeCsv;

        private bool divideRecord;
        private int idSave;

        public Form1() {
            this.InitializeComponent();
            this.InitBrowser("ati.su", "http://ati.su/");
            this.InitBrowser("info", "about:blank");
            this.InitBrowser("blist", "about:blank");
        }

        private string stepDate(double daysPlus, TextBox currentTB) {

            convertDF = Convert.ToDateTime(currentTB.Text);

            currentTB.Text = convertDF.AddDays(daysPlus).ToShortDateString();
            currentTB.Text = currentTB.Text;
            return currentTB.Text;

        }

        private DateTime upgradeInvoke(double daysPlus, TextBox tb, DateTime date_from) {
            this.convertTbText_date_from = date_from;
            convertTbText_date_from = Convert.ToDateTime(tb.Text);
            if (testFlag) {
                readyConvertTbText_date_from = convertTbText_date_from.AddDays(daysPlus);
            }

            tb.Text = readyConvertTbText_date_from.ToShortDateString(); //обновляю tbText так прост
            return readyConvertTbText_date_from;
        }


        private static DateTime toDateTime(TextBox tb) {

            return Convert.ToDateTime(tb.Text);
        }

        private int Rand(int from, int to) //возвращяет целое число в указаном диапазоне
        {
            return new Random().Next(from, to);
        } //NO

        // public delegate string myDelegate(string str);

        public void AppendTextBox(string value) //вставляет значение в консоль
        {
            string s1 = null;
            //myDelegate delegsMethod = param => {
            //  return "kek" + param;
            //};
            //s1 = delegsMethod("grek");

            string newtext = "";
            this.textBox_log.Invoke((Action)(() =>
            {
                newtext = DateTime.Now.ToString("HH:mm:ss") + " - " + value + "\r\n"; //местное время стринг + значение
                this.textBox_log.Text = newtext + this.textBox_log.Text;
                string[] strArray = this.textBox_log.Text.Split(new string[1]
                {
          "\r\n"
                }, StringSplitOptions.RemoveEmptyEntries);
                string str = "";
                int num = strArray.Length;
                if (num > 200)
                    num = 200;
                for (int index = 0; index < num; ++index)
                    str = str + strArray[index] + "\r\n";
                this.textBox_log.Text = str;
            }));
            StreamWriter streamWriter = new StreamWriter("log " + this.log_time + ".txt", true, Encoding.Default);
            streamWriter.Write(newtext);
            streamWriter.Close(); //записывает все в фаил txt
        }

        private long GetTime() {
            DateTime dateTime = new DateTime();
            DateTime now = DateTime.Now;
            TimeSpan timeSpan = new TimeSpan();
            return Convert.ToInt64(now.Subtract(new DateTime(1970, 1, 1, 4, 0, 0)).TotalMilliseconds);
        }                                                            //NO

        public double ToDouble(string val) {
            char newChar = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            return Convert.ToDouble(val.Replace(',', newChar).Replace('.', newChar));
        }                                                //NO

        private int ToInteger(string val) //конвертирует в int 32 значение
        {
            return Convert.ToInt32(val);
        }

        public bool IsNumber(string word) {
            double result;
            return double.TryParse(word, out result);
        }                                                 //NO

        private void SaveSettings() {
            Form1.Settings settings = new Form1.Settings();
            settings.login = this.textBox_login.Text;
            settings.pass = this.textBox_pass.Text;
            settings.city = this.textBox_city.Text;
            settings.ball = this.comboBox_ball.Text;
            settings.profile = this.comboBox_profile.Text;
            settings.SstepString = this.combo_box_step.Text;
            settings.date_from = this.textBox_date_from.Text;
            settings.date_to = this.textBox_date_to.Text;
            settings.docs = this.textBox_docs.Text;
            settings.textOfCity = this.textBoxCitys.Text;
            settings.recomend = this.textBox_recomend.Text;
            settings.contact = this.textBox_contact.Text;
            settings.blackList1 = this.checkBox_blaclist1.Checked;
            settings.blackList2 = this.checkBox_blaclist2.Checked;
            settings.blackList3 = this.checkBox_blaclist3.Checked;
            settings.onlyIP = this.checkBox_onlyIP.Checked;
            settings.SearchDeleted = this.checkBox_SearchDeleted.Checked;
            settings.IsNoActivity = this.checkBox_IsNoActivity.Checked;
            settings.NoActivityNum = this.comboBox_NoActivity.Text;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Form1.Settings));
            TextWriter textWriter = (TextWriter)new StreamWriter(Application.StartupPath + "\\settings.xml");
            xmlSerializer.Serialize(textWriter, (object)settings);
            textWriter.Close();
            this.AppendTextBox("Настройки сохранены");
        }     //сохраняет настройки в xml файле

        private void LoadSettings() {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Form1.Settings));
            Form1.Settings settings;
            try {
                FileStream fileStream = new FileStream(Application.StartupPath + "\\settings.xml", FileMode.Open);
                settings = (Form1.Settings)xmlSerializer.Deserialize((Stream)fileStream); //файлик xml
                fileStream.Close();
            }
            catch {
                this.AppendTextBox("Файл настроек отсутствует");
                return;
            }

            this.textBox_login.Text = settings.login;
            this.textBox_pass.Text = settings.pass;
            this.textBox_city.Text = settings.city;
            this.comboBox_ball.Text = settings.ball;
            this.comboBox_profile.Text = settings.profile;
            this.combo_box_step.Text = settings.SstepString;
            this.textBox_date_from.Text = settings.date_from;
            this.textBox_date_to.Text = settings.date_to;
            if (settings.docs == null)
                settings.docs = "";
            this.textBox_docs.Text = settings.docs.Replace("\n", "\r\n");

            if (settings.textOfCity == null)
                settings.textOfCity = "";
            this.textBoxCitys.Text = settings.textOfCity.Replace("\n", "\r\n");

            this.textBox_recomend.Text = settings.recomend;
            this.textBox_contact.Text = settings.contact;
            this.checkBox_blaclist1.Checked = settings.blackList1;
            this.checkBox_blaclist2.Checked = settings.blackList2;
            this.checkBox_blaclist3.Checked = settings.blackList3;
            this.checkBox_onlyIP.Checked = settings.onlyIP;
            this.checkBox_SearchDeleted.Checked = settings.SearchDeleted;
            this.checkBox_IsNoActivity.Checked = settings.IsNoActivity;
            this.comboBox_NoActivity.Text = settings.NoActivityNum;

        }     //загружает эти самые настройки

        public int CookiesClear() {
            Form1.Task_ClearCookies taskClearCookies = new Form1.Task_ClearCookies();
            CefRuntime.PostTask(CefThreadId.IO, (CefTask)taskClearCookies);
            this.Pause(1500);
            return taskClearCookies.count;
        }                                                         //NO

        /*protected override void Dispose(bool disposing)                                      //NO
        {
          if (disposing && this.components != null)
            this.components.Dispose();
          base.Dispose(disposing);
        }*/

        private void InitializeComponent() {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.запуститьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.остановитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.textBox_city = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_date_from = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_date_to = new System.Windows.Forms.TextBox();
            this.comboBox_ball = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_log = new System.Windows.Forms.TextBox();
            this.textBox_login = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_pass = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_docs = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_recomend = new System.Windows.Forms.TextBox();
            this.textBox_contact = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox_profile = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.checkBox_blaclist1 = new System.Windows.Forms.CheckBox();
            this.checkBox_blaclist2 = new System.Windows.Forms.CheckBox();
            this.checkBox_blaclist3 = new System.Windows.Forms.CheckBox();
            this.checkBox_onlyIP = new System.Windows.Forms.CheckBox();
            this.checkBox_SearchDeleted = new System.Windows.Forms.CheckBox();
            this.comboBox_NoActivity = new System.Windows.Forms.ComboBox();
            this.checkBox_IsNoActivity = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.combo_box_step = new System.Windows.Forms.ComboBox();
            this.textBoxCitys = new System.Windows.Forms.TextBox();
            this.city_label = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.запуститьToolStripMenuItem,
            this.остановитьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(998, 24);
            this.menuStrip1.TabIndex = 38;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // запуститьToolStripMenuItem
            // 
            this.запуститьToolStripMenuItem.Name = "запуститьToolStripMenuItem";
            this.запуститьToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.запуститьToolStripMenuItem.Text = "Запустить";
            this.запуститьToolStripMenuItem.Click += new System.EventHandler(this.запуститьToolStripMenuItem_Click);
            // 
            // остановитьToolStripMenuItem
            // 
            this.остановитьToolStripMenuItem.Name = "остановитьToolStripMenuItem";
            this.остановитьToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.остановитьToolStripMenuItem.Text = "Остановить";
            this.остановитьToolStripMenuItem.Click += new System.EventHandler(this.остановитьToolStripMenuItem_Click);
            // 
            // tabControl
            // 
            this.tabControl.Location = new System.Drawing.Point(12, 26);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(980, 365);
            this.tabControl.TabIndex = 252;
            // 
            // textBox_city
            // 
            this.textBox_city.Location = new System.Drawing.Point(68, 427);
            this.textBox_city.Name = "textBox_city";
            this.textBox_city.Size = new System.Drawing.Size(184, 20);
            this.textBox_city.TabIndex = 265;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 427);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 264;
            this.label4.Text = "Город";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 457);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 266;
            this.label1.Text = "Балл";
            // 
            // textBox_date_from
            // 
            this.textBox_date_from.Location = new System.Drawing.Point(68, 517);
            this.textBox_date_from.Name = "textBox_date_from";
            this.textBox_date_from.Size = new System.Drawing.Size(76, 20);
            this.textBox_date_from.TabIndex = 269;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 513);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 268;
            this.label2.Text = "Дата с";
            // 
            // textBox_date_to
            // 
            this.textBox_date_to.Location = new System.Drawing.Point(174, 517);
            this.textBox_date_to.Name = "textBox_date_to";
            this.textBox_date_to.Size = new System.Drawing.Size(78, 20);
            this.textBox_date_to.TabIndex = 271;
            // 
            // comboBox_ball
            // 
            this.comboBox_ball.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ball.FormattingEnabled = true;
            this.comboBox_ball.Items.AddRange(new object[] {
            "",
            "Не менее 1 \"звезды\"",
            "Не менее 2 \"звёзд\"",
            "Не менее 3 \"звёзд\"",
            "Не менее 4 \"звёзд\"",
            "5 \"звёзд\""});
            this.comboBox_ball.Location = new System.Drawing.Point(68, 457);
            this.comboBox_ball.Name = "comboBox_ball";
            this.comboBox_ball.Size = new System.Drawing.Size(184, 21);
            this.comboBox_ball.TabIndex = 272;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(152, 517);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 13);
            this.label6.TabIndex = 273;
            this.label6.Text = "по";
            // 
            // textBox_log
            // 
            this.textBox_log.Location = new System.Drawing.Point(264, 397);
            this.textBox_log.Multiline = true;
            this.textBox_log.Name = "textBox_log";
            this.textBox_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_log.Size = new System.Drawing.Size(728, 172);
            this.textBox_log.TabIndex = 42;
            this.textBox_log.TabStop = false;
            // 
            // textBox_login
            // 
            this.textBox_login.Location = new System.Drawing.Point(68, 397);
            this.textBox_login.Name = "textBox_login";
            this.textBox_login.Size = new System.Drawing.Size(67, 20);
            this.textBox_login.TabIndex = 275;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 397);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 274;
            this.label5.Text = "Логин";
            // 
            // textBox_pass
            // 
            this.textBox_pass.Location = new System.Drawing.Point(186, 397);
            this.textBox_pass.Name = "textBox_pass";
            this.textBox_pass.Size = new System.Drawing.Size(67, 20);
            this.textBox_pass.TabIndex = 277;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(139, 400);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 276;
            this.label7.Text = "Пароль";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 645);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 278;
            this.label8.Text = "Документы:";
            // 
            // textBox_docs
            // 
            this.textBox_docs.Location = new System.Drawing.Point(12, 662);
            this.textBox_docs.Multiline = true;
            this.textBox_docs.Name = "textBox_docs";
            this.textBox_docs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_docs.Size = new System.Drawing.Size(236, 87);
            this.textBox_docs.TabIndex = 280;
            this.textBox_docs.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 577);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(133, 13);
            this.label9.TabIndex = 281;
            this.label9.Text = "Кол-во рекомендаций >=";
            // 
            // textBox_recomend
            // 
            this.textBox_recomend.Location = new System.Drawing.Point(173, 577);
            this.textBox_recomend.Name = "textBox_recomend";
            this.textBox_recomend.Size = new System.Drawing.Size(78, 20);
            this.textBox_recomend.TabIndex = 282;
            this.textBox_recomend.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox_contact
            // 
            this.textBox_contact.Location = new System.Drawing.Point(173, 607);
            this.textBox_contact.Name = "textBox_contact";
            this.textBox_contact.Size = new System.Drawing.Size(78, 20);
            this.textBox_contact.TabIndex = 284;
            this.textBox_contact.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 607);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(111, 13);
            this.label10.TabIndex = 283;
            this.label10.Text = "Кол-во контактов <=";
            // 
            // comboBox_profile
            // 
            this.comboBox_profile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_profile.FormattingEnabled = true;
            this.comboBox_profile.Items.AddRange(new object[] {
            "",
            "Грузовладелец",
            "Грузовладелец-перевозчик",
            "Диспетчер",
            "Перевозчик",
            "Экспедитор",
            "Экспедитор-перевозчик",
            "IT компания",
            "Автосервис",
            "Страховая компания"});
            this.comboBox_profile.Location = new System.Drawing.Point(68, 487);
            this.comboBox_profile.MaxDropDownItems = 10;
            this.comboBox_profile.Name = "comboBox_profile";
            this.comboBox_profile.Size = new System.Drawing.Size(184, 21);
            this.comboBox_profile.TabIndex = 286;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 483);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 285;
            this.label11.Text = "Профиль";
            // 
            // checkBox_blaclist1
            // 
            this.checkBox_blaclist1.AutoSize = true;
            this.checkBox_blaclist1.Location = new System.Drawing.Point(594, 577);
            this.checkBox_blaclist1.Name = "checkBox_blaclist1";
            this.checkBox_blaclist1.Size = new System.Drawing.Size(398, 17);
            this.checkBox_blaclist1.TabIndex = 287;
            this.checkBox_blaclist1.Text = "Искать в подразделе Контакты записей в: Недобросовестные партнеры";
            this.checkBox_blaclist1.UseVisualStyleBackColor = true;
            // 
            // checkBox_blaclist2
            // 
            this.checkBox_blaclist2.AutoSize = true;
            this.checkBox_blaclist2.Location = new System.Drawing.Point(594, 597);
            this.checkBox_blaclist2.Name = "checkBox_blaclist2";
            this.checkBox_blaclist2.Size = new System.Drawing.Size(280, 17);
            this.checkBox_blaclist2.TabIndex = 288;
            this.checkBox_blaclist2.Text = "Искать в подразделе Контакты: на форумах ati.su";
            this.checkBox_blaclist2.UseVisualStyleBackColor = true;
            // 
            // checkBox_blaclist3
            // 
            this.checkBox_blaclist3.AutoSize = true;
            this.checkBox_blaclist3.Location = new System.Drawing.Point(594, 617);
            this.checkBox_blaclist3.Name = "checkBox_blaclist3";
            this.checkBox_blaclist3.Size = new System.Drawing.Size(319, 17);
            this.checkBox_blaclist3.TabIndex = 289;
            this.checkBox_blaclist3.Text = "Искать в подразделе Контакты: на форумах auto-trust.info";
            this.checkBox_blaclist3.UseVisualStyleBackColor = true;
            // 
            // checkBox_onlyIP
            // 
            this.checkBox_onlyIP.AutoSize = true;
            this.checkBox_onlyIP.Location = new System.Drawing.Point(264, 577);
            this.checkBox_onlyIP.Name = "checkBox_onlyIP";
            this.checkBox_onlyIP.Size = new System.Drawing.Size(120, 17);
            this.checkBox_onlyIP.TabIndex = 290;
            this.checkBox_onlyIP.Text = "Искать только ИП";
            this.checkBox_onlyIP.UseVisualStyleBackColor = true;
            // 
            // checkBox_SearchDeleted
            // 
            this.checkBox_SearchDeleted.AutoSize = true;
            this.checkBox_SearchDeleted.Location = new System.Drawing.Point(264, 597);
            this.checkBox_SearchDeleted.Name = "checkBox_SearchDeleted";
            this.checkBox_SearchDeleted.Size = new System.Drawing.Size(178, 17);
            this.checkBox_SearchDeleted.TabIndex = 291;
            this.checkBox_SearchDeleted.Text = "Учитывать удаленные фирмы";
            this.checkBox_SearchDeleted.UseVisualStyleBackColor = true;
            // 
            // comboBox_NoActivity
            // 
            this.comboBox_NoActivity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_NoActivity.FormattingEnabled = true;
            this.comboBox_NoActivity.Items.AddRange(new object[] {
            "1 месяц",
            "2 месяца",
            "3 месяца",
            "4 месяца",
            "5 месяцев",
            "6 месяцев"});
            this.comboBox_NoActivity.Location = new System.Drawing.Point(400, 615);
            this.comboBox_NoActivity.Name = "comboBox_NoActivity";
            this.comboBox_NoActivity.Size = new System.Drawing.Size(93, 21);
            this.comboBox_NoActivity.TabIndex = 293;
            // 
            // checkBox_IsNoActivity
            // 
            this.checkBox_IsNoActivity.AutoSize = true;
            this.checkBox_IsNoActivity.Location = new System.Drawing.Point(264, 617);
            this.checkBox_IsNoActivity.Name = "checkBox_IsNoActivity";
            this.checkBox_IsNoActivity.Size = new System.Drawing.Size(130, 17);
            this.checkBox_IsNoActivity.TabIndex = 294;
            this.checkBox_IsNoActivity.Text = "Нулевая активность";
            this.checkBox_IsNoActivity.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 547);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(27, 13);
            this.label12.TabIndex = 295;
            this.label12.Text = "Шаг";
            // 
            // combo_box_step
            // 
            this.combo_box_step.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_box_step.FormattingEnabled = true;
            this.combo_box_step.Items.AddRange(new object[] {
            "не использовать",
            "1 день",
            "2 дня",
            "3 дня",
            "7 дней",
            "14 дней",
            "31 день",
            "60 дней",
            "90 дней",
            "120 дней",
            "150 дней",
            "180 дней"});
            this.combo_box_step.Location = new System.Drawing.Point(68, 547);
            this.combo_box_step.Name = "combo_box_step";
            this.combo_box_step.Size = new System.Drawing.Size(184, 21);
            this.combo_box_step.TabIndex = 296;
            // 
            // textBoxCitys
            // 
            this.textBoxCitys.Location = new System.Drawing.Point(264, 662);
            this.textBoxCitys.Multiline = true;
            this.textBoxCitys.Name = "textBoxCitys";
            this.textBoxCitys.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxCitys.Size = new System.Drawing.Size(728, 87);
            this.textBoxCitys.TabIndex = 297;
            this.textBoxCitys.TabStop = false;
            // 
            // city_label
            // 
            this.city_label.AutoSize = true;
            this.city_label.Location = new System.Drawing.Point(264, 645);
            this.city_label.Name = "city_label";
            this.city_label.Size = new System.Drawing.Size(54, 13);
            this.city_label.TabIndex = 298;
            this.city_label.Text = "Заметки:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 753);
            this.Controls.Add(this.city_label);
            this.Controls.Add(this.textBoxCitys);
            this.Controls.Add(this.combo_box_step);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.checkBox_IsNoActivity);
            this.Controls.Add(this.comboBox_NoActivity);
            this.Controls.Add(this.checkBox_SearchDeleted);
            this.Controls.Add(this.checkBox_onlyIP);
            this.Controls.Add(this.checkBox_blaclist3);
            this.Controls.Add(this.checkBox_blaclist2);
            this.Controls.Add(this.checkBox_blaclist1);
            this.Controls.Add(this.comboBox_profile);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox_contact);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox_recomend);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox_docs);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox_pass);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_login);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox_ball);
            this.Controls.Add(this.textBox_date_to);
            this.Controls.Add(this.textBox_date_from);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_city);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.textBox_log);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ati.su";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void InitBrowser(string TabName, string startUrl) {
            label_begin:
            TabPage tabPage = new TabPage(TabName);
            tabPage.Name = TabName;
            tabPage.Padding = new Padding(0, 0, 0, 0);
            CefWebBrowser cefWebBrowser = new CefWebBrowser();
            cefWebBrowser.StartUrl = startUrl;
            cefWebBrowser.Dock = DockStyle.Fill;
            cefWebBrowser.BrowserSettings = new CefBrowserSettings();
            cefWebBrowser.BeforePopup += (EventHandler<BeforePopupEventArgs>)((s, e) =>
            {
                this.webControl.LoadUrl(e.TargetUrl);
                e.Handled = true;
            });
            tabPage.Controls.Add((Control)cefWebBrowser);
            this.tabControl.TabPages.Add(tabPage);
        } //инициализирует браузер

        private CefWebBrowser GetBrowser(string TabName) {
            if (this.tabControl.TabCount > 0) {
                foreach (object control in (ArrangedElementCollection)this.tabControl.TabPages[TabName].Controls) {
                    if (control is CefWebBrowser)
                        return (CefWebBrowser)control;
                }
            } //возвраящет браузер
            return (CefWebBrowser)null;
        }

        private void Pause(int msec) {
            int num = 0;
            do {
                Application.DoEvents();
                Thread.Sleep(100);
                ++num;
            }
            while (num <= msec / 100);
        } //ставит на паузу thread

        private void Form1_Load(object sender, EventArgs e) {
            this.client = new MyCefStringVisitor();
            this.LoadSettings();
        } //инициализиурет cefStringVisitor и заполняет поля программы при ее запуске

        private void запуститьToolStripMenuItem_Click(object sender, EventArgs e) //запускает основной поток при нажатии кнопки
        {
            this.combo_box_step.Invoke((Action)(() => stepText = this.combo_box_step.Text));
            this.thread_main = new Thread(new ThreadStart(this.Monitor));
            this.thread_main.CurrentUICulture = new CultureInfo("en-US");
            this.thread_main.Start();
            testFlag = false; //чтобы не увеличил шаг сразу
            without_step = true;
            dontJumpPlsMoreStep = false;
            makeDate_from_one_time = true;
            createOneTimeCsv = true;
        }

        private void Monitor() {
            //convertDT = DateTime.Now;
            /*
            if
            string[] cityArray = new string[2] {
                 "Москва",
                 "Казань"
            };

            List<string> cityList = ((IEnumerable<string>)docs.Split(new string[1]
                    {
          "\r\n"
                    }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();

            foreach (string cityName in cityArray) { */
            
            if (stepText != "не использовать") {
            goto label_begin;
            }

            label_begin:

            for (readyConvertTbText_date_from = toDateTime(textBox_date_from);
            readyConvertTbText_date_from < toDateTime(textBox_date_to);
            readyConvertTbText_date_from.AddDays(0)) {
                divideRecord = true;
                //чтобы не зацикликвался когда без шага и чтобы не прыгал лишнего, даже если dateForm меньше даты textbox_date_do 
                if (!without_step & stepText == "не использовать" || dontJumpPlsMoreStep ) { 
                    this.IsMonitor = false;
                    this.thread_main.Abort();
                    this.запуститьToolStripMenuItem.Enabled = true;
                    this.AppendTextBox("Поиск остановлен!");
                }
                //если без шага и
                if (without_step & stepText == "не использовать") {

                    goto label_without_step;
                }
                label_without_step:

                try {
                    this.IsMonitor = true;
                    //при нажатии на "запустить" 
                    this.menuStrip1.Invoke((Action)(() => this.запуститьToolStripMenuItem.Enabled = false));//делает кнопку не активной
                    this.AppendTextBox("Поиск запущен ...");
                    this.tabControl.Invoke((Action)(() => {
                        for (int index = 0; index < this.tabControl.TabPages.Count; ++index) {
                            this.tabControl.SelectedTab = this.tabControl.TabPages[index];
                            this.Pause(100);
                        }
                        this.tabControl.SelectedTab = this.tabControl.TabPages[0];//после запуска возвращяет на 1 - ый таб
                    }));
                    ///////////////////////////////////////////////////сохраняет в переменые данные с полей
                    string login = "";
                    this.textBox_login.Invoke((Action)(() => login = this.textBox_login.Text));
                    string pass = "";
                    this.textBox_pass.Invoke((Action)(() => pass = this.textBox_pass.Text));
                    string city = "";
                    this.textBox_city.Invoke((Action)(() => city = this.textBox_city.Text));
                    string ball = "";
                    this.comboBox_ball.Invoke((Action)(() => ball = this.comboBox_ball.Text));
                    string profile = "";
                    this.comboBox_profile.Invoke((Action)(() => profile = this.comboBox_profile.Text));
                    if (profile == "Грузовладелец")
                        profile = "3";
                    if (profile == "Грузовладелец-перевозчик")
                        profile = "6";
                    if (profile == "Диспетчер")
                        profile = "4";
                    if (profile == "Перевозчик")
                        profile = "1";
                    if (profile == "Экспедитор")
                        profile = "2";
                    if (profile == "Экспедитор-перевозчик")
                        profile = "5";
                    if (profile == "IT компания")
                        profile = "23";
                    if (profile == "Автосервис")
                        profile = "22";
                    if (profile == "Страховая компания")
                        profile = "7";


                    string stringStepDouble = "";
                    this.combo_box_step.Invoke((Action)(() => stringStepDouble = this.combo_box_step.Text));
                    if (stringStepDouble == "не использовать")
                        stepDouble = 0;
                    if (stringStepDouble == "1 день")
                        stepDouble = 1;
                    if (stringStepDouble == "2 дня")
                        stepDouble = 2;
                    if (stringStepDouble == "3 дня")
                        stepDouble = 3;
                    if (stringStepDouble == "7 дней")
                        stepDouble = 7;
                    if (stringStepDouble == "14 дней")
                        stepDouble = 14;
                    if (stringStepDouble == "31 день")
                        stepDouble = 31;
                    if (stringStepDouble == "60 дней")
                        stepDouble = 60;
                    if (stringStepDouble == "90 дней")
                        stepDouble = 90;
                    if (stringStepDouble == "120 дней")
                        stepDouble = 120;
                    if (stringStepDouble == "150 дней")
                        stepDouble = 150;
                    if (stringStepDouble == "180 дней")
                        stepDouble = 180;








                    bool BL1 = false;
                    this.checkBox_blaclist1.Invoke((Action)(() => BL1 = this.checkBox_blaclist1.Checked));
                    bool BL2 = false;
                    this.checkBox_blaclist2.Invoke((Action)(() => BL2 = this.checkBox_blaclist2.Checked));
                    bool BL3 = false;
                    this.checkBox_blaclist3.Invoke((Action)(() => BL3 = this.checkBox_blaclist3.Checked));
                    bool onlyIP = false;
                    this.checkBox_onlyIP.Invoke((Action)(() => onlyIP = this.checkBox_onlyIP.Checked));
                    bool SearchDeleted = false;
                    this.checkBox_SearchDeleted.Invoke((Action)(() => SearchDeleted = this.checkBox_SearchDeleted.Checked));

                    if (makeDate_from_one_time) { //чтобы файл csv создавался только один раз
                        this.textBox_date_from.Invoke((Action)(() => date_from = this.textBox_date_from.Text));
                        makeDate_from_one_time = false;
                    }


                    //this.textBox_date_from.Invoke((Action) (() => date_from = stepDate(2, textBox_date_from)));
                    this.textBox_date_from.Invoke((Action)(() => dayFrom = upgradeInvoke(stepDouble, textBox_date_from, convertTbText_date_from)));

                    //это нужно чтобы если шаг = 0, поиск не зацикливался, т.к dayFrom не будет увеличиваться, следовательно и dayTo
                    if (stepDouble == 0) {
                        this.textBox_date_to.Invoke((Action)(() => dayTo = Convert.ToDateTime(textBox_date_to.Text)));
                    }
                    else {
                        this.textBox_date_to.Invoke((Action)(() => dayTo = dayFrom.AddDays(stepDouble)));
                    }

                    //это я делаю,чтобы он не перепрыгивал лишнего!
                    DateTime date_to = DateTime.Now;
                    this.textBox_date_to.Invoke((Action)(() => date_to = Convert.ToDateTime(textBox_date_to.Text)));

                    if (dayFrom.AddDays(stepDouble) > date_to) {//чтобы не перепрыгивал лишнего

                        dayTo = date_to;
                        dontJumpPlsMoreStep = true;
                        //TimeSpan diff = date_to.Subtract(dayFrom);
                        //stepDouble = diff.Days;
                    }
                    /////////////////////////////////////////////////

                    //this.textBox_date_to.Invoke((Action) (() => date_to = this.textBox_date_to.Text));
                    string docs = "";
                    this.textBox_docs.Invoke((Action)(() => docs = this.textBox_docs.Text));

                    //List<string> docsList = new List<string>();
                    List<string> listDoc = ((IEnumerable<string>)docs.Split(new string[1]
                    {
          "\r\n"
                    }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
                    int recomend = 1;
                    this.textBox_recomend.Invoke((Action)(() => recomend = Convert.ToInt32(this.textBox_recomend.Text)));
                    int contacts = 1;
                    this.textBox_contact.Invoke((Action)(() => contacts = Convert.ToInt32(this.textBox_contact.Text)));
                    bool IsNoActivity = false;
                    string IsNoActivityNum = "";
                    this.textBox_contact.Invoke((Action)(() => {
                        IsNoActivity = this.checkBox_IsNoActivity.Checked;
                        IsNoActivityNum = this.comboBox_NoActivity.Text;
                    }));
                    //////////////////////////////////////////////сохраняет в переменые данные с полей

                    //string path = "firms " + DateTime.Now.ToString("dd MMMM yyyy HHmmss") + ".csv"; //создание файла csv 
                    string dateNow = DateTime.Now.ToShortDateString();
                    if (without_step & stepText == "не использовать") {
                        
                        path = "firms " + dayFrom.ToLongDateString() + " - " + dayTo.ToLongDateString() + "(" + dateNow + ")" + ".csv";
                    }
                    else if (createOneTimeCsv && stepText != "не использовать") {
                        //path = "firms " + dayFrom.ToShortDateString() + " - " + dayFrom.AddDays(stepDouble).ToShortDateString() + ".csv";
                        DateTime date_from_date = Convert.ToDateTime(date_from);
                        path = "firms " + date_from_date.ToLongDateString() + " - " + date_to.ToLongDateString() + "(" + dateNow + ")" + ".csv";
                    }

                    if (divideRecord) { 
                    StreamWriter streamWriter1 = new StreamWriter(path, true, Encoding.Default); //записывальщик данных
                    string str1 = dayFrom.ToShortDateString() + " - " + dayTo.ToShortDateString();
                    streamWriter1.Write(str1 + "\r\n");
                    streamWriter1.Close();
                        divideRecord = false;
                    }

                    //загружает в окно страницу
                    this.webControl = this.GetBrowser("ati.su").Browser.GetMainFrame();
                    this.webControl.LoadUrl("http://ati.su/");
                    this.Pause(3000);

                    //проверяет есть ли авторизация и если нет, то авторизуется
                    if (this.CheckLoad("1").Contains("Вход в систему &gt;&gt;")) {
                        this.AppendTextBox("Требуется авторизация!");
                        this.webControl.ExecuteJavaScript("$('a:contains(\"Вход в систему >>\")')[0].click()", (string)null, 0);
                        this.Pause(3000);
                        this.webControl.ExecuteJavaScript("$('input[id=\"lgnUserName\"]')[0].value='" + login + "'", (string)null, 0);
                        this.webControl.ExecuteJavaScript("$('input[id=\"lgnPassword\"]')[0].value='" + pass + "'", (string)null, 0);
                        this.webControl.ExecuteJavaScript("$('input[id=\"lgnbtnPopupLogin\"]')[0].click();", (string)null, 0);
                        this.Pause(3000);
                    }

                    this.CheckLoad("Выйти из системы");
                    this.AppendTextBox("Вы авторизованы!");
                    this.Pause(3000);
                    this.webControl.ExecuteJavaScript("$('a:contains(\"Участники АвтоТрансИнфо\")')[0].click()", (string)null, 0);
                    this.Pause(3000);
                    this.AppendTextBox("ждем 5 сек, чтобы нас не заподозрили злые админы");
                    this.Pause(5000);


                    if (SearchDeleted)//если стоит галка удаленые фирмы
                        this.webControl.ExecuteJavaScript("$('input[id=\"ctl00_main_chkDeleted\"]')[0].checked=true;", (string)null, 0);
                    else
                        this.webControl.ExecuteJavaScript("$('input[id=\"ctl00_main_chkDeleted\"]')[0].checked=false;", (string)null, 0);
                    this.webControl.ExecuteJavaScript("document.getElementById('ctl00_main_atxtCity_TextBox').select();", null, 0);
                    Pause(500);
                    this.webControl.ExecuteJavaScript("$('input[id=\"ctl00_main_atxtCity_TextBox\"]')[0].value='" + city + "'", (string)null, 0);
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    this.webControl.ExecuteJavaScript("document.getElementById('ctl00_main_atxtCity_TextBox').select();", null, 0);
                    Pause(500);
                    this.webControl.ExecuteJavaScript("$('input[id=\"ctl00_main_atxtCity_TextBox\"]')[0].value='" + city + "'", (string)null, 0);
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    this.webControl.ExecuteJavaScript("document.getElementById('ctl00_main_atxtCity_TextBox').select();", null, 0);
                    Pause(1000);
                    this.webControl.ExecuteJavaScript("$('input[id=\"ctl00_main_atxtCity_TextBox\"]')[0].value='" + city + "'", (string)null, 0);
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //this.webControl.ExecuteJavaScript("document.getElementById('ctl00_main_atxtCity_TextBox').value = 5", null, 0);
                    this.webControl.ExecuteJavaScript("$('select[id=\"ctl00_main_ddlReliability\"]')[0].value='" + ball + "'", (string)null, 0);
                    this.webControl.ExecuteJavaScript("$('select[id=\"ctl00_main_ddlProfile\"]')[0].value='" + profile + "'", (string)null, 0);
                    this.webControl.ExecuteJavaScript("$('input[id=\"ctl00_main_tbRegDateFrom\"]')[0].value='" + dayFrom.ToShortDateString() + "'", (string)null, 0);
                    this.webControl.ExecuteJavaScript("$('input[id=\"ctl00_main_tbRegDateTo\"]')[0].value='" + dayTo.ToShortDateString() + "'", (string)null, 0);
                    testFlag = true;
                    without_step = false;
                    createOneTimeCsv = false;
                    this.webControl.ExecuteJavaScript("$('input[name=\"ctl00$main$btnSearch\"]')[0].click()", (string)null, 0);
                    //идет вставка данных в поиск
                    int num1 = 0;
                    HtmlNodeCollection htmlNodeCollectionFirms; //Представляет объединенный список и коллекцию узлов html. (Список найденых фирм)
                    bool flag1;
                    //ждет 10 секунд в ожидании результатов первоначального поиска, либо говорит что найдено более 100 фирм
                    do //если коллекция фирм  пуста 
                    {
                        HtmlNode htmlNodeWindowListFirm; //Представляет конкретный узел html. (окошко фирм)
                        do //если узел пуст, то выбирает следующий узел
                        {
                            flag1 = true;
                            ++num1;
                            if (num1 <= 10) { //пока не прошло 10 секунд
                                this.webControl.GetSource((CefStringVisitor)this.client); //Получить HTML-код этого фрейма в виде строки,
                                                                                          //отправленной указанному посетителю
                                this.Pause(1000);
                                string currentHtml = this.client.Html; //теущая страница html

                                //не содержит!
                                if (!currentHtml.Contains("Найдено более 100 фирм")) {
                                    HtmlNode.ElementsFlags.Remove((object)"option");//удаляет элемент с хэш таблицы
                                    HtmlAgilityPack.HtmlDocument htmlDocFirmList = new HtmlAgilityPack.HtmlDocument(); //предоставляет ссылку на полный html документ
                                    htmlDocFirmList.LoadHtml(currentHtml); //помещает текущую страницу в htmlDocFirmList
                                    htmlNodeWindowListFirm = htmlDocFirmList.DocumentNode.SelectSingleNode("//select[@id='ctl00_main_lstResult']");//окошко со списком фирм
                                }
                                else
                                    goto label_27; //скажи что найдено более 100 фирм в логах

                            }
                            else
                                goto label_25; //скажи время ожидания результатов поиска истекло!
                        } while (htmlNodeWindowListFirm == null); //если окошко фирмы пусто

                        htmlNodeCollectionFirms = htmlNodeWindowListFirm.SelectNodes(".//option"); //выбирает колекцию под - тегов тега option
                    }
                    while (htmlNodeCollectionFirms == null);
                    ///////////////////////////////////////////////////////////////////////////////////////////////////

                    goto label_31;

                    label_25:
                    this.AppendTextBox("Время ожидания результатов поиска истекло!");
                    goto label_97; //завершающая стадия
                    label_27:
                    this.AppendTextBox("Найдено более 100 фирм!");
                    goto label_97; ////завершающая стадия

                    label_31: //возвращяет количество найденых фирм и выводит в лог
                    string detectedFirms = "Обнаружено фирм - ";
                    int countFirm = htmlNodeCollectionFirms.Count; //количество фирм
                    this.AppendTextBox(detectedFirms + countFirm);

                    
                    for (int index1 = 0; index1 < htmlNodeCollectionFirms.Count; ++index1) //чето делает пока index 1 меньше количества найденых фирм
                    {
                        this.tabControl.Invoke((Action)(() => this.tabControl.SelectedTab = this.tabControl.TabPages[0]));
                        //берет по индексу значения фирмы
                        string valueOfFirm = htmlNodeCollectionFirms[index1].Attributes["value"].Value;
                        string aliasOfFirm = htmlNodeCollectionFirms[index1].Attributes["aliasid"].Value;
                        //////////////////////////////

                        string nameFirm = htmlNodeCollectionFirms[index1].InnerText; //текст название найденой фирмы 

                        if (!onlyIP || nameFirm.Contains(" ИП")) {
                            object[] objArray1 = new object[7] //массив который говорит какая по счету обрабатывается фирма и сколько осталось фирм
                            {
              (object) "Обрабатывается фирма - ",
              (object) nameFirm,
              (object) " (",
              null,
              null,
              null,
              null
                            };
                            object[] objArray2 = objArray1;
                            countFirm = index1 + 1;
                            string countFirmToStr = countFirm.ToString();
                            objArray2[3] = (object)countFirmToStr;
                            objArray1[4] = (object)" из ";
                            objArray1[5] = (object)htmlNodeCollectionFirms.Count;
                            objArray1[6] = (object)")";
                            this.AppendTextBox(string.Concat(objArray1)); //объединяет все значения строкового массива
                                                                          //массив который говорит какая по счету обрабатывается фирма и сколько осталось фирм

                            //code                                                              url        line
                            this.webControl.ExecuteJavaScript("$('select[id=\"ctl00_main_lstResult\"]')[0].value='" + valueOfFirm + "'", (string)null, 0);//просто выделяет фирму по value of firm
                            this.webControl.ExecuteJavaScript("$('select[id=\"ctl00_main_lstResult\"]').change();", (string)null, 0);//жмет на выделеную фирму
                                                                                                                                     //ждет загрузки первоначальной информации о фирме
                            string htmlStringFirstPage = this.CheckLoad("<span id=\"lblAtiCode\" style=\"font-weight: bold;\" class=\"firmInf\">" + aliasOfFirm + "</span>");
                            //ждет загрузки первоначальной информации о фирме (а точнее кода АТИ)
                            this.Pause(1000);

                            if (htmlStringFirstPage == "")
                                this.AppendTextBox("Истекло время ожидания для Фирма");
                            else if (htmlStringFirstPage.Contains("<span style='color:red'")) {
                                this.AppendTextBox("Обнаружен красный балл!");
                            }
                            else {

                                //переключает табы в которых открывает опеределеный url
                                HtmlAgilityPack.HtmlDocument objectForHtmlFirstPage = new HtmlAgilityPack.HtmlDocument(); //предоставляет хранить хтмлдок || ссылку на полный html документ
                                objectForHtmlFirstPage.LoadHtml(htmlStringFirstPage);
                                string dataRegAti = objectForHtmlFirstPage.DocumentNode.SelectSingleNode("//span[@id='lblRegDate']").InnerText;
                                string urlOfFirmDetalis = objectForHtmlFirstPage.DocumentNode.SelectSingleNode("//span[@id='aFirmDetails']").SelectSingleNode(".//a").Attributes["href"].Value;
                                this.webControl_info = this.GetBrowser("info").Browser.GetMainFrame();
                                this.webControl_info.LoadUrl("about:blank");
                                this.Pause(1000);
                                this.webControl_info.LoadUrl("http://ati.su" + urlOfFirmDetalis);
                                this.tabControl.Invoke((Action)(() => this.tabControl.SelectedTab = this.tabControl.TabPages[1]));
                                //переключает табы в которых открывает опеределеный url

                                //ждет загрузки раздела "Контакты" фирмы (а точнее Название фирмы) а так же загрузки тега "PassportIframe"
                                this.CheckLoadInfo("Название фирмы", "PassportIframe");

                                string currentHtml = this.CheckLoadInfo("rptContact_ctl", "PassportIframe");
                                if (currentHtml == "") //html2
                                    this.AppendTextBox("Истекло время ожидания для Контакты");
                                else if (BL1 && currentHtml.Contains("lnkFirmMentionSearch")) //если в блек листе и еще что то (?)
                                {
                                    this.AppendTextBox("Обнаружены записи в Недобросовестные партнеры");
                                }
                                else {
                                    HtmlAgilityPack.HtmlDocument htmlDocument2 = new HtmlAgilityPack.HtmlDocument();
                                    htmlDocument2.LoadHtml(currentHtml);
                                    HtmlNode TagDoc = htmlDocument2.DocumentNode.SelectSingleNode("//div[@id='panelFirmDocuments']");//выбирает тег документов
                                    List<string> stringList2 = new List<string>();
                                    if (TagDoc != null) {
                                        HtmlNodeCollection htmlNodeCollection2 = TagDoc.SelectNodes(".//a");
                                        for (int index3 = 1; index3 < htmlNodeCollection2.Count; ++index3)
                                            stringList2.Add(htmlNodeCollection2[index3].InnerText);
                                    }
                                    bool flag2 = false;
                                    for (int index3 = 0; index3 < stringList2.Count; ++index3) {
                                        for (int index4 = 0; index4 < listDoc.Count; ++index4) {
                                            if (stringList2[index3].ToLower().Contains(listDoc[index4].ToLower())) {
                                                flag2 = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (listDoc.Count != 0 && !flag2) {
                                        this.AppendTextBox("Документы не найдены!");
                                    }
                                    else {
                                        string mobileNumberOrNumberOfJustTelephone = "";
                                        HtmlNodeCollection htmlNodeCollection2 = htmlDocument2.DocumentNode.SelectNodes("//div[contains(@id,'rptContact_ctl')]");
                                        //если есть мобильный номер то сохраняет в mobileNumberOrNumberOfJustTelephone
                                        if (htmlNodeCollection2[0].SelectSingleNode(".//span[@id='rptContact_ctl00_lblMobile']") != null)
                                            mobileNumberOrNumberOfJustTelephone = htmlNodeCollection2[0].SelectSingleNode(".//span[@id='rptContact_ctl00_lblMobile']").InnerText;
                                        //если есть номер телефона то сохраняет в mobileNumberOrNumberOfJustTelephone
                                        else if (htmlNodeCollection2[0].SelectSingleNode(".//span[@id='rptContact_ctl00_lblPhone']") != null) //lblPhone просто Телефон
                                            mobileNumberOrNumberOfJustTelephone = htmlNodeCollection2[0].SelectSingleNode(".//span[@id='rptContact_ctl00_lblPhone']").InnerText;
                                        int int32 = Convert.ToInt32(contacts);
                                        if (htmlNodeCollection2.Count > int32) {
                                            this.AppendTextBox("Контактов больше " + (object)int32 + "!");
                                        }
                                        else {
                                            this.webControl_info.ExecuteJavaScript("$('a:contains(\"Паспорт\")')[0].click();", (string)null, 0);
                                            string html3 = this.CheckLoadInfo("Итоговый балл:", "PassportIframe");
                                            if (IsNoActivity)
                                                html3 = this.CheckLoadInfo("chartDataUserSiteActivityChart", "PassportIframe");
                                            if (html3 == "") {
                                                this.AppendTextBox("Истекло время ожидания для Паспорт");
                                            }
                                            else {
                                                htmlDocument2.LoadHtml(html3);
                                                HtmlNode htmlNode2 = htmlDocument2.DocumentNode.SelectSingleNode("//div[@id='divRecsForMeContain']");
                                                if (htmlNode2 != null) {
                                                    int num3 = htmlNode2.InnerText.LastIndexOf("(");
                                                    int num4 = htmlNode2.InnerText.LastIndexOf(")");
                                                    if (Convert.ToInt32(htmlNode2.InnerText.Substring(num3 + 1, num4 - num3 - 1)) < recomend) {
                                                        this.AppendTextBox("Рекомендаций меньше " + (object)recomend + "!");
                                                        continue;
                                                    }
                                                }
                                                else if (recomend != 0) {
                                                    this.AppendTextBox("Рекомендации не найдены!");
                                                    continue;
                                                }
                                                if (IsNoActivity) {
                                                    int integer = this.ToInteger(IsNoActivityNum.Substring(0, 1));
                                                    int num3 = 0;
                                                    int startIndex1 = 0;
                                                    int num4 = 0;
                                                    while (true) {
                                                        flag1 = true;
                                                        int startIndex2 = html3.IndexOf("\"data\"", startIndex1);
                                                        ++num4;
                                                        if (num4 <= 6) {
                                                            int startIndex3 = html3.IndexOf("[", startIndex2);
                                                            int num5 = html3.IndexOf("]", startIndex3);
                                                            List<string> list2 = ((IEnumerable<string>)html3.Substring(startIndex3 + 1, num5 - startIndex3 - 1).Split(new string[1]
                                                            {
                              ","
                                                            }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
                                                            for (int index3 = list2.Count - 1; index3 >= list2.Count - integer; --index3)
                                                                num3 += this.ToInteger(list2[index3]);
                                                            startIndex1 = num5 + 1;
                                                        }
                                                        else
                                                            break;
                                                    }
                                                    if (num3 != 0) {
                                                        this.AppendTextBox("Профиль был активен!");
                                                        continue;
                                                    }
                                                }
                                                this.tabControl.Invoke((Action)(() => this.tabControl.SelectedTab = this.tabControl.TabPages[2]));
                                                this.webControl_bl = this.GetBrowser("blist").Browser.GetMainFrame();
                                                if (BL2) {
                                                    label_81:
                                                    this.webControl_bl.LoadUrl("about:blank");
                                                    this.Pause(1000);
                                                    flag3 = false;
                                                    this.webControl_bl.LoadUrl("http://ati.su/Forum/Search.aspx?forumid=0&options=4&firmid=" + aliasOfFirm);
                                                    int num3 = 0;
                                                    string html4;
                                                    do {
                                                        flag1 = true;
                                                        ++num3;
                                                        if (num3 <= 10) {
                                                            this.webControl_bl.GetSource((CefStringVisitor)this.client);
                                                            this.Pause(1000);
                                                            html4 = this.client.Html;
                                                            if (html4.Contains("Нет записей, соответствующих вашему запросу."))
                                                                goto label_86;
                                                        }
                                                        else
                                                            goto label_81;
                                                    }
                                                    while (!html4.Contains("Результатов поиска:"));
                                                    this.AppendTextBox("ati.su/Forum: найдены записи!");
                                                    flag3 = true;
                                                    label_86:
                                                    if (flag3)
                                                        continue;
                                                }
                                                label_87:
                                                while (BL3) {
                                                    this.webControl_bl.LoadUrl("about:blank");
                                                    this.Pause(1000);
                                                    bool flag3 = false;
                                                    this.webControl_bl.LoadUrl("http://auto-trust.info/Forum/Search.aspx?forumid=0&options=4&firmid=" + aliasOfFirm);
                                                    int num3 = 0;
                                                    string html4;
                                                    do {
                                                        flag1 = true;
                                                        ++num3;
                                                        if (num3 <= 10) {
                                                            this.webControl_bl.GetSource((CefStringVisitor)this.client);
                                                            this.Pause(1000);
                                                            html4 = this.client.Html;
                                                            if (html4.Contains("Нет записей, соответствующих вашему запросу."))
                                                                goto label_93;
                                                        }
                                                        else
                                                            goto label_87;
                                                    }
                                                    while (!html4.Contains("Результатов поиска:"));
                                                    this.AppendTextBox("auto-trust.info/Forum: найдены записи!");
                                                    flag3 = true;
label_93:
                                                    if (!flag3) //если флаг ложь то не записывает
                                                        break;
                                                    goto label_96;
                                                }
                                                StreamWriter streamWriter2 = new StreamWriter(path, true, Encoding.Default);
                                                string str9 = dataRegAti + ";" + aliasOfFirm + ";" + mobileNumberOrNumberOfJustTelephone;
                                                streamWriter2.Write(str9 + "\r\n");
                                                streamWriter2.Close();
                                                
                                                this.AppendTextBox("Фирма добавлена!");

                                                label_96:
                                                if (!flag3) { 
                                                //this.AppendTextBox("Фирма не добавлена, т.к найдены записи");
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    //this.textBox_date_to.Invoke((Action)(() => date_to = stepDate(2, textBox_date_to)));


                    label_97:
                    this.AppendTextBox("Поиск завершен!");
                    this.menuStrip1.Invoke((Action)(() => this.запуститьToolStripMenuItem.Enabled = true));


                }

                catch (Exception ex) {
                    if (this.IsMonitor) {
                        this.AppendTextBox("Ошибка: " + ex.Message + "\r\nТрассировка: " + ex.StackTrace); //если файл excel открыт то выходит такая ошибка
                        this.menuStrip1.Invoke((Action)(() => this.запуститьToolStripMenuItem.Enabled = true));
                    }
                }
            }   //my 
            //}
        }

        private void остановитьToolStripMenuItem_Click(object sender, EventArgs e) {
            this.IsMonitor = false;
            this.thread_main.Abort();
            this.запуститьToolStripMenuItem.Enabled = true;
            this.AppendTextBox("Поиск остановлен!");
        }

        private string CheckLoad(string val) {
            this.client.Html = ""; //строки страницы html

            DateTime now = DateTime.Now; //текущее время
            string html;
            do {
                if (DateTime.Now.Subtract(now).TotalSeconds <= 20.0) //вычитает из текущего времени время в секундах и если оно меньше или равно 20 то
                {
                    //webControl предоставляет фрейм в окне браузера
                    this.webControl.GetSource((CefStringVisitor)this.client); //GetSorce()Получить HTML-код этого фрейма в виде строки,
                                                                              //отправленной указанному посетителю.
                    this.Pause(1000);
                    html = this.client.Html;

                }
                else
                    return "";
            }
            while (!html.Contains(val)); //делай пока страница не содержит определеное значение возвращяй строки странцы 
            return html;   //иначе пустую строку

        } //если в течении 20 секунд  не содержит val, то то возвращяет страницу(как бы думает), иначе пустую строку
          //и пока есть фрейм



        private string CheckLoadInfo(string val, string frame)//если в течении 20 секунд  не содержит val, то то возвращяет страницу(как бы думает)

    {
            this.client.Html = "";
            DateTime now = DateTime.Now;
            string html;
            do {
                do {
                    if (DateTime.Now.Subtract(now).TotalSeconds <= 10.0)
                        this.Pause(1000);
                    else
                        return "";
                }
                while (this.webControl_info.Browser.GetFrame(frame) == null);
                this.webControl_info.Browser.GetFrame(frame).GetSource((CefStringVisitor)this.client);
                html = this.client.Html;
            }
            while (!html.Contains(val));
            return html;


        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            this.IsMonitor = false;
            this.SaveSettings();
            if (this.thread_main == null)
                return;
            this.thread_main.Abort();
        }

        public class Settings {
            public string login;
            public string pass;
            public string city;
            public string ball;
            public string profile;
            public string date_from;
            public string date_to;
            public string docs;
            public string recomend;
            public string contact;
            public bool blackList1;
            public bool blackList2;
            public bool blackList3;
            public bool onlyIP;
            public bool SearchDeleted;
            public bool IsNoActivity;
            public string NoActivityNum;
            public string SstepString;
            public string textOfCity;
            public string idSaveFile;
        }

        private class MyCookieVisitor : CefCookieVisitor {
            public List<CefCookie> cookie_List = new List<CefCookie>();
            private int _total;

            public int total {
                get {
                    return this._total;
                }
                set {
                    this._total = value;
                }
            }

            protected override bool Visit(CefCookie cookie, int count, int total, out bool c) {
                this._total = total;
                c = false;
                this.cookie_List.Add(cookie);
                return true;
            }
        }

        private class Task_ClearCookies : CefTask {
            public bool IsClear = false;
            public int count = -1;
            private List<CefCookie> cookies = new List<CefCookie>();

            protected override void Execute() {
                CefCookieManager.Global.DeleteCookies("", "");
                Form1.MyCookieVisitor myCookieVisitor = new Form1.MyCookieVisitor();
                CefCookieManager.Global.VisitAllCookies((CefCookieVisitor)myCookieVisitor);
                this.IsClear = true;
                this.count = myCookieVisitor.total;
                this.cookies = myCookieVisitor.cookie_List;
            }
        }

        /*private DateTime upgradeInvoke(double daysPlus, TextBox tb, DateTime date_from) {
            this.convertTbText_date_from = date_from;
            convertTbText_date_from = Convert.ToDateTime(tb.Text);
            readyConvertTbText_date_from = convertTbText_date_from.AddDays(daysPlus);
            tb.Text = readyConvertTbText_date_from.ToShortDateString(); //обновляю tbText
            return readyConvertTbText_date_from;
        }
        */
    }
}
