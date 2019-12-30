using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZKFPEngXControl;
using _4500BioMetricApp.Model;
using _4500BioMetricApp.DAL;
using System.ComponentModel;
using System.Threading;
using _4500BioMetricApp.Misc;
using ZKFPEngXControl;
using System.Reflection;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using WindowsInput;
using WindowsInput.Native;
 

namespace _4500BioMetricApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        InstituteDAL insDAL;
        EmployeeDAL empDAL;
        List<sms_employee> emp_list;
        List<sms_emp_thumb_enroll> thumb_enroll_list;
        ZKFPEngX fp;
        object pTemplate;
        string pTemplateString;
        object aTemplate;
        string aTemplateString;
        sms_emp_thumb_enroll thumb_enroll_obj;
        bool isAttendance = false;
        sms_emp_attendance attendance_obj;
        string mode = "";
        string mode_text = "";
        List<sms_emp_attendance> attendance_list;
        bool isInitialized = false;
        DispatcherTimer screenSaverPlaylistTimer;
        ImageSliderWPF.MainWindow m_ScreenSaverWindow;
        sms_institute ins;
        public DispatcherTimer m_DigitalClockTimer;

        public static string Database = "sms";
        public static string Server = "localhost";
        public static string Port = "3306";
        public static string Uid = "root";

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                ReadDatabaseFile();
                insDAL = new InstituteDAL();
                empDAL = new EmployeeDAL();                

                digital_clock();
                TabControlMain.SelectedIndex = 0;                
                ins = insDAL.get_sms_institute();
                institute_grid.DataContext = ins;
                image_grid.DataContext = ins;                

                
            }
            catch (Exception ex) 
            {
                error_grid.Visibility = Visibility.Visible;
                error_grid.DataContext = ex.Message;
                isInitialized = false;
            }
        }

        #region Screen Saver playlist

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }
        void screenSaverPlaylistTimer_Tick(object sender, EventArgs e)
        {
            if (GetIdleTime() > 0.2 * 60) // 10 min
            {
                m_ScreenSaverWindow.Show();
                m_ScreenSaverWindow.Activate();
            }
            else
            {
                m_ScreenSaverWindow.Hide();
            }
                //mainWindow.Activate();
        }

        void fp_OnFingerTouching()
        {
            InputSimulator inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.RETURN);
        }

        #endregion
        long GetIdleTime()
        {
            var info = new LASTINPUTINFO { cbSize = (uint)Marshal.SizeOf(typeof(LASTINPUTINFO)) };
            GetLastInputInfo(ref info);
            return (Environment.TickCount - info.dwTime) / 1000;
        }       
        public void initEngine()
        {
            try
            {
                fp = new ZKFPEngXControl.ZKFPEngX();                
                if (fp.InitEngine() == 0)
                {

                    isInitialized = true;
                    fp.OnImageReceived += new IZKFPEngXEvents_OnImageReceivedEventHandler(fp_OnImageReceived);
                    fp.OnEnroll += new IZKFPEngXEvents_OnEnrollEventHandler(fp_OnEnroll);
                    fp.OnCapture += new IZKFPEngXEvents_OnCaptureEventHandler(fp_OnCapture);
                    fp.OnFingerTouching += fp_OnFingerTouching;
                    
                    error_grid.Visibility = Visibility.Collapsed;
                }
                else
                {
                    error_grid.Visibility = Visibility.Visible;
                    error_grid.DataContext = "Failed To Initialize Device";
                    isInitialized = false;
                    fp.EndEngine();                    
                }
            }
            catch (Exception ex)
            {
                error_grid.Visibility = Visibility.Visible;
                error_grid.DataContext = ex.Message;
            }
        }
        public void endEngine()
        {
            try
            {
                fp.EndEngine();
            }
            catch (Exception ex)
            {
                error_grid.Visibility = Visibility.Visible;
                error_grid.DataContext = ex.Message;
            }
        }   

        void digital_clock()         
        {
            m_DigitalClockTimer = new DispatcherTimer();
            m_DigitalClockTimer.Interval = new TimeSpan(0, 0, 1); // 100 Milliseconds  
            m_DigitalClockTimer.Tick += myDispatcherTimer_Tick;
            m_DigitalClockTimer.Start();

            date_TB.Text = DateTime.Today.ToString("dd-MMM-yyyy");
        }
        void myDispatcherTimer_Tick(object sender, EventArgs e)
        {
            tbk_clock.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }  
        void fp_OnImageReceived(ref bool AImageValid)
        {
            if (isAttendance == false)
            {
                if (AImageValid)
                {
                    object imgdata = new object();
                    bool b = fp.GetFingerImage(ref imgdata);
                    string filename = DateTime.Now.ToString("yyyyMMddHHmmss-" + thumb_enroll_obj.emp_id) + ".jpg";
                    fp.SaveJPG(filename);
                    string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var imagePath = path + @"\" + filename;
                    //finger_image.Source = new BitmapImage(new Uri(imagePath));

                    int index = fp.EnrollIndex;
                    int quality = fp.LastQuality;

                    if (fp.EnrollIndex == 3)
                    {
                        thumb_enroll_obj.quality1 = fp.LastQuality;
                        thumb_enroll_obj.image1 = (byte[])File.ReadAllBytes(imagePath);
                    }
                    if (fp.EnrollIndex == 2)
                    {
                        thumb_enroll_obj.quality2 = fp.LastQuality;
                        thumb_enroll_obj.image2 = (byte[])File.ReadAllBytes(imagePath);
                    }
                    if (fp.EnrollIndex == 1)
                    {
                        thumb_enroll_obj.quality3 = fp.LastQuality;
                        thumb_enroll_obj.image3 = (byte[])File.ReadAllBytes(imagePath);
                    }

                    employee_finger_GBs.DataContext = null;
                    employee_finger_GBs.DataContext = thumb_enroll_obj;



                    //operation_text.Text = "Please Enroll Finger  " + fp.EnrollIndex + " Times More, Current Quality " + fp.LastQuality;
                    //enroll_count_text.Text = fp.EnrollIndex.ToString();
                    //MemoryStream stream = new MemoryStream();
                    //finger_img.Source = ByteToImage(obj.image);       
                }
                else 
                {
                    MessageBox.Show("Image Not Valid");
                }
            }
        }

        void employee_finger_GBs_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            employee_finger_GBs.DataContext = thumb_enroll_obj;
        }
        void fp_OnEnroll(bool ActionResult, object ATemplate)
        {
            try
            {
                if (ActionResult)
                {

                    pTemplate = new object();
                    pTemplate = ATemplate;
                    pTemplateString = fp.GetTemplateAsString();


                    fill_object();
                    if (empDAL.insert_thumb_enroll(thumb_enroll_obj) > 0)
                    {
                        show_status_verify();                        
                    }
                    else
                    {
                        MessageBox.Show("Insertion Failed");
                        show_status_Unverify();
                    }
                    //fp.ControlSensor(13,1);
                }
                else
                {
                    show_status_Unverify();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                show_status_Unverify();
            }
            fp.CancelEnroll();
            fp.CancelCapture();
        }

        public void fill_object() 
        {
            thumb_enroll_obj.emp_id = Convert.ToInt32( (emp_listBox.SelectedItem as sms_employee).id);
            thumb_enroll_obj.emp_login_id = 0;
            thumb_enroll_obj.created_by = "Tahir";
            thumb_enroll_obj.date_time = DateTime.Now;
            thumb_enroll_obj.finger_type_id = 1;
            thumb_enroll_obj.hand = "right";
            thumb_enroll_obj.templateStr = fp.GetTemplateAsString();      
        }

        void fp_OnCapture(bool ActionResult, object ATemplate)
        {
            try
            {
                mode="";
                bool check = false;
                if (ActionResult)
                {
                    aTemplate = new object();
                    aTemplate = ATemplate;
                    aTemplateString = fp.GetTemplateAsString();

                    foreach (var item in thumb_enroll_list)
                    {
                        check = false;
                        pTemplateString = Convert.ToBase64String(item.template);
                        if (fp.VerFingerFromStr(pTemplateString, aTemplateString, true, true))
                        {
                            employee_att_details_GBs.DataContext = null;
                            //fill object
                            attendance_obj = new sms_emp_attendance();
                            attendance_obj.emp_id = item.emp_id;
                            if(empDAL.get_last_attendance_mode(item.emp_id) == "checkin")
                            {
                                mode="checkout";
                                mode_text="You Are Out";
                            }
                            else
                            {
                                mode = "checkin";
                                mode_text="You Are In";
                            }

                            attendance_obj.mode = mode;
                            attendance_obj.emp_login_id = 0;
                            attendance_obj.date_time = DateTime.Now;
                            attendance_obj.created_by = "Tahir";

                            if(empDAL.insert_thumb_attendance(attendance_obj)> 0)
                            {
                                employee_att_details_GBs.DataContext = emp_list.Where(x => x.id == item.emp_id.ToString()).First();
                                mode_text_TB.Text = mode_text;
                                check = true;
                                error_grid.Visibility = Visibility.Collapsed;
                                load_emp_att_history();
                                break;
                            }                            
                        }
                    }
                    if (check == false)
                    {
                        employee_att_details_GBs.DataContext = null;
                        error_grid.Visibility = Visibility.Visible;
                        error_grid.DataContext = "Not Verified Please Try Again";
                        mode_text_TB.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Capture Failed");
                }
            }catch(Exception ex)
            { MessageBox.Show(ex.Message); }
        }


        private void TabControlClasses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if(TabControlMain.SelectedItem != null)
            {
                initEngine();
                if (TabControlMain.SelectedIndex == 0)
                {
                    load_attendance();
                }
                else 
                {
                    load_enroll();
                }
            }
            e.Handled = true;
        }

        public void load_attendance()         
        {
            try
            {
                isAttendance = true;
                empDAL = new EmployeeDAL();
                emp_list = empDAL.get_all_active_employees();
                thumb_enroll_list = empDAL.get_all_thumb_enroll();
                employee_att_details_GBs.DataContext = null;
                mode_text_TB.Text = "";
                if (isInitialized)
                {
                    fp.BeginCapture();
                }
                hide_status();
                load_emp_att_history();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        
        public void load_enroll()
        {
            try
            {
                isAttendance = false;
                hide_status();
                employee_details_GBs.DataContext = null;
                emp_list = empDAL.get_all_active_employees();
                thumb_enroll_obj = new sms_emp_thumb_enroll();
                foreach (var item in emp_list)
                {
                    item.icon = MetroMenuResources.Logo.User;
                }
                emp_listBox.ItemsSource = emp_list;

                if (isInitialized)
                {
                    fp.BeginEnroll();
                }
                else
                {
                    employee_finger_GBs.Visibility = Visibility.Collapsed;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void search_TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            emp_listBox.ItemsSource = emp_list.Where(x => x.emp_name.ToUpper().Contains(search_TB.Text.ToUpper()));
        }

        public void show_status_verify() 
        {
            status_grid_verify.Visibility = Visibility.Visible;
            status_grid_unVerify.Visibility = Visibility.Collapsed;
            error_grid.Visibility = Visibility.Collapsed;
        }
        public void show_status_Unverify()
        {
            status_grid_verify.Visibility = Visibility.Collapsed;
            status_grid_unVerify.Visibility = Visibility.Visible;
            error_grid.Visibility = Visibility.Collapsed;
        }
        public void hide_status()
        {
            status_grid_verify.Visibility = Visibility.Collapsed;
            status_grid_unVerify.Visibility = Visibility.Collapsed;
            //error_grid.Visibility = Visibility.Collapsed;

        }


#region Waiting bar
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            worker.RunWorkerAsync();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbStatus.Visibility = Visibility.Collapsed;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {            
            
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
        }

#endregion

        private void emp_listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (emp_listBox.SelectedItem != null)
            {
                thumb_enroll_obj = new sms_emp_thumb_enroll();
                employee_finger_GBs.DataContext = null;

                sms_employee emp = emp_listBox.SelectedItem as sms_employee;
                employee_details_GBs.DataContext = emp;

                employee_finger_GBs.Visibility = Visibility.Visible;

                thumb_enroll_obj.emp_id = Convert.ToInt32(emp.id);
                hide_status();
                fp.BeginEnroll();
                fp.BeginCapture();
                e.Handled = true;

                thumb_enroll_obj = empDAL.get_all_thumb_enroll_by_emp_id(Convert.ToInt32(emp.id));
                employee_finger_GBs.DataContext = thumb_enroll_obj;
            }
            else 
            {
                employee_finger_GBs.Visibility = Visibility.Collapsed;
            }
        }

        private void try_again_btn_Click(object sender, RoutedEventArgs e)
        {
            thumb_enroll_obj = new sms_emp_thumb_enroll();
            employee_finger_GBs.DataContext = null;
            sms_employee emp = emp_listBox.SelectedItem as sms_employee;
            thumb_enroll_obj.emp_id = Convert.ToInt32(emp.id);
            hide_status();            
            fp.BeginEnroll();
            fp.BeginCapture();
        }

        public void load_emp_att_history()         
        {
            attendance_list = empDAL.get_all_attendance_by_date(DateTime.Today);
            emp_history_listBox.ItemsSource = attendance_list;            
        }

        private void mainWindow_Closed(object sender, EventArgs e)
        {
            m_ScreenSaverWindow.Close();
            this.Close();
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread.Sleep(5000);
                m_ScreenSaverWindow = new ImageSliderWPF.MainWindow();
                screenSaverPlaylistTimer = new DispatcherTimer();
                screenSaverPlaylistTimer.Tick += screenSaverPlaylistTimer_Tick;
                screenSaverPlaylistTimer.Interval = new TimeSpan(0, 0, 1);
                screenSaverPlaylistTimer.Start();
                m_ScreenSaverWindow.SetImageLogo(ins.institute_logo);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mainWindow_Activated(object sender, EventArgs e)
        {
            if (m_DigitalClockTimer != null)
            {
                m_DigitalClockTimer.Start();
            }
        }

        private void mainWindow_Deactivated(object sender, EventArgs e)
        {
            if (m_DigitalClockTimer != null)
            {
                m_DigitalClockTimer.Stop();
            }            
        }

        void ReadDatabaseFile()
        {
            string line;
            int i = 0;
            var fileName = "Database.txt";
            var spFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var folderPath = System.IO.Path.Combine(spFolderPath, "ScenarioSystems", "4500BioMetric App");
            var filePath = System.IO.Path.Combine(folderPath, fileName);

            try
            {
                //using (StreamReader sr = new StreamReader(spFolderPath + "Database.txt"))
                using (StreamReader sr = new StreamReader(filePath))

                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (i == 0 && line.Trim() != "")
                        {
                            Server = line;
                        }
                        if (i == 1 && line.Trim() != "")
                        {
                            Port = line;
                        }
                        if (i == 2 && line.Trim() != "")
                        {
                            Database = line;
                        }
                        if (i == 3 && line.Trim() != "")
                        {
                            Uid = line;
                        }
                        i++;
                    }

                }
            }
            catch (Exception e)
            {
                Directory.CreateDirectory(folderPath);
                using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(folderPath, fileName)))
                {
                    outputFile.WriteLine("localhost");
                    outputFile.WriteLine("3306");
                    outputFile.WriteLine("sms");
                    outputFile.WriteLine("root");
                }
            }
        }
    }
}
