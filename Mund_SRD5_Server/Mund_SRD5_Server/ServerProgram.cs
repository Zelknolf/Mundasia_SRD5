using Mundasia.Objects;
using Mundasia.Server.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mundasia
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Service.Open();
            LoadLocalResources();
            Map.LoadedMaps.Add("Material", new Map() { Name = "Material" });
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Status());
            Service.Close();
        }

        static void LoadLocalResources()
        {
            Race.Load();
            Skill.Load();
            Background.Load();
        }
    }
}
