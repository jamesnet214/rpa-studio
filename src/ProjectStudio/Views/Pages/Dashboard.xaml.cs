using RoslynPad.Editor;
using RoslynPad.Roslyn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace ProjectStudio.Views.Pages
{
    /// <summary>
    /// Main.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();

            Loaded += Dashboard_Loaded;
        }

        private RoslynHost _host;


        private void Dashboard_Loaded(object sender, RoutedEventArgs e)
        {
            _host = new RoslynHost(additionalAssemblies: new[]
          {
                Assembly.Load("Project.Roslyn.Windows"),
                Assembly.Load("Project.Editor.Windows")
            }, RoslynHostReferences.NamespaceDefault.With(assemblyReferences: new[]
          {
                typeof(object).Assembly,
                typeof(System.Text.RegularExpressions.Regex).Assembly,
                typeof(System.Linq.Enumerable).Assembly,
            }));

            var workingDirectory = Directory.GetCurrentDirectory();
            Editor.Initialize(_host, new ClassificationHighlightColors(), workingDirectory, string.Empty, Microsoft.CodeAnalysis.SourceCodeKind.Script);

        }

        private void Editor_OnLoaded(object sender, RoutedEventArgs e)
        {
            _ = Dispatcher.InvokeAsync(() => Editor.Focus(), System.Windows.Threading.DispatcherPriority.Background);
        }
    }
}
