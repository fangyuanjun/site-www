using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Blogs.UI.Main
{
    public class MemCache
    {
        static readonly object padlock = new object();
        //线程安全的单例模式
        public static MemcachedClient Instance
        {
            get
            {
                lock (padlock)
                {
                    CacheDependency depend = new CacheDependency(HttpContext.Current.Server.MapPath("~/Web.config"));
                    Cache cache = HttpRuntime.Cache;
                    MemcachedClient instance = cache["memcached"] as MemcachedClient;
                    if (instance == null)
                    {
                        instance = new MemcachedClient("memcached");
                        cache.Insert("memcached", instance, depend);
                    }

                    //MemcachedClient mc;
                    //MemcachedClientConfiguration config = new MemcachedClientConfiguration();
                    //config.AddServer("127.0.0.1:11211");
                    //config.Protocol = MemcachedProtocol.Binary;
                    //mc = new MemcachedClient(config);

                    return instance;
                }
            }
        }

        //阿里云示例
        //static void MemClientInit()
        //{
        //    //初始化缓存
        //    MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();
        //    // IPAddress newaddress = IPAddress.Parse(Dns.GetHostEntry("a8b43b75331f4877.m.cnszalist3pub001.ocs.aliyuncs.com").AddressList[0].ToString()); //xxxx替换为ocs控制台上的“内网地址”
        //    IPAddress newaddress = IPAddress.Parse(Dns.GetHostEntry("127.0.0.1").AddressList[0].ToString());
        //    IPEndPoint ipEndPoint = new IPEndPoint(newaddress, 11211);
        //    // 配置文件 - ip
        //    memConfig.Servers.Add(ipEndPoint);
        //    // 配置文件 - 协议
        //    memConfig.Protocol = MemcachedProtocol.Binary;
        //    // 配置文件-权限，如果使用了免密码功能，则无需设置userName和password
        //    memConfig.Authentication.Type = typeof(PlainTextAuthenticator);
        //    memConfig.Authentication.Parameters["zone"] = "";
        //    memConfig.Authentication.Parameters["userName"] = "a8b43b75331f4877";
        //    memConfig.Authentication.Parameters["password"] = "xiaofangfang88AA";
        //    //下面请根据实例的最大连接数进行设置
        //    memConfig.SocketPool.MinPoolSize = 5;
        //    memConfig.SocketPool.MaxPoolSize = 200;
        //    MemClient = new MemcachedClient(memConfig);
        //}
    }
}
