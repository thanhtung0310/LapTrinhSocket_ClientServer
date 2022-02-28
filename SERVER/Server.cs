using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
namespace SERVER
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;//tránh việc đụng độ khi sử dụng tài nguyên giữa các thread
            Connect();
        }

        IPEndPoint IP;
        Socket server;

        //kết nối đến server
        void Connect()
        {
            //khởi tạo địa chỉ IP và socket để kết nối
            IP = new IPEndPoint(IPAddress.Any, 1997);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            //đợi kết nối từ client
            server.Bind(IP);
            //tạo 1 luồng lăng nghe từ client
            Thread Listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        server.Listen(100);
                        Socket client = server.Accept();//nếu lăng nghe thành công thì server chấp nhận kết nối
                        //tạo luồng nhận thông tin từ client
                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(client);
                    }
                }
                catch
                {
                    IP = new IPEndPoint(IPAddress.Any, 1997);
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                }
            });
            Listen.IsBackground = true;
            Listen.Start();
        }

        public Boolean KT;
        public string string1;

        //sử dụng thuật toán sắp xếp chọn
        void Sort(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
                for (int j = i + 1; j < arr.Length; j++)
                    if (arr[i] > arr[j])
                    {
                        int temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
        }

        //Hàm xử lý chuỗi bao gồm các công việc:
        string Process(string str)
        {
            KT = true;
            //loại bỏ đi các ký tự khoảng trắng thừa ở đầu và cuối chuỗi
            str = str.Trim();
            //loại bỏ đi các ký tự khoảng trắng thừa ở trong chuỗi
            while (str.IndexOf("  ") != -1)
            {
                str = str.Replace("  ", " ");
            }
            //tách các phần tử số trong chuỗi
            string[] x = str.Split(' ');
            string s = " ";
            //Kiểm tra xem trong chuỗi chỉ có tồn tại số và ép kiểu sang các số nguyên
            int[] result = new int[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                if (Regex.IsMatch(x[i], "[0-9]") == true
                        && Regex.IsMatch(x[i], @"^[-+]?[0-9]\.?[0-9]+$") == false)
                    result[i] = Convert.ToInt32(x[i]);
                else
                {
                    KT = false;
                    break;
                }
            }
            //sắp xếp chuỗi vừa thu được
            Sort(result);
            //đưa ra chuỗi dưới dạng string
            for (int i = 0; i < x.Length; i++)
            {
                s = s + Convert.ToString(result[i]) + " ";
            }
            return s;
        }

        //thêm message vào khung chat
        void AddMessage(string s)
        {
            lsvMessage.Items.Add(new ListViewItem() { Text = s });
        }

        //nhận dữ liệu
        void Receive(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                while (true)
                {
                    //khởi tạo mảng byte để nhận dữ liệu
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);
                    //chuyển data từ dạng byte sang dạng string
                    string message = Convert.ToString(Deserialize(data));
                    string1 = "";
                    //xử lý chuỗi vừa nhận được
                    string1 = Process(message);
                    {
                        //nếu chuỗi nhập vào đúng định dạng thì gửi lại chuỗi đã sắp xếp về client và hiển thị lên màn hình server
                        if (KT == true)
                        {
                            client.Send(Serialize("Server: " + string1));
                            AddMessage("Client: " + message); 
                        }
                        //nếu chuỗi nhập vào sai định dạng thì gửi thông báo lỗi về client và hiển thị lên màn hinh server
                        else
                        {
                            client.Send(Serialize("Server: Chuỗi nhập vào sai định dạng"));
                            AddMessage("Client: Chuỗi nhập vào sai định dạng");
                        }
                    }
                }
            }
            catch
            {
                //đóng luồng
                client.Close();
            }
        }

        //đóng kết nối đến server
        void Close()
        {
            server.Close();
        }

        //Hàm phân mảnh dữ liệu cần gửi từ dạng string sang dạng byte để gửi đi
        byte[] Serialize(object obj)
        {
            //khởi tạo stream để lưu các byte phân mảnh
            MemoryStream stream = new MemoryStream();
            //khởi tạo đối tượng BinaryFormatter để phân mảnh dữ liệu sang kiểu byte
            BinaryFormatter formatter = new BinaryFormatter();
            //phân mảnh rồi ghi vào stream
            formatter.Serialize(stream, obj);
            //từ stream chuyển các các byte thành dãy rồi cbi gửi đi
            return stream.ToArray();
        }

        //Hàm gom mảnh các byte nhận được rồi chuyển sang kiểu string để hiện thị lên màn hình
        object Deserialize(byte[] data)
        {
            //khởi tạo stream đọc kết quả của quá trình phân mảnh 
            MemoryStream stream = new MemoryStream(data);
            //khởi tạo đối tượng chuyển đổi
            BinaryFormatter formatter = new BinaryFormatter();
            //chuyển đổi dữ liệu và lưu lại kết quả 
            return formatter.Deserialize(stream);
        }
    }
}
