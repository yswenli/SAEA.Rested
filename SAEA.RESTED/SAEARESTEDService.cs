using SAEA.MVC;
using System.ServiceProcess;

namespace SAEA.RESTED
{
    partial class SAEARESTEDService : ServiceBase
    {
        SAEAMvcApplication _mvcApplication;

        public SAEARESTEDService()
        {
            InitializeComponent();

            var mvcApplicationConfig = SAEAMvcApplicationConfigBuilder.Read();

            _mvcApplication = new SAEAMvcApplication(mvcApplicationConfig);
        }

        protected override void OnStart(string[] args)
        {
            _mvcApplication.Start();
        }

        protected override void OnStop()
        {
            _mvcApplication.Stop();
        }

        public void Run()
        {
            ServiceBase[] ServicesToRun =
            {
                new SAEARESTEDService()
            };
            Run(ServicesToRun);
        }

    }
}
