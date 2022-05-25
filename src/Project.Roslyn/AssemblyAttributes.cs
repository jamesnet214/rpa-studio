using System.Runtime.CompilerServices;

// 아래 어셈블리에 현재 프로젝트 내부 클래스를 노출시킨다.
[assembly: InternalsVisibleTo("Project")]
[assembly: InternalsVisibleTo("Project.Common.UI")]
[assembly: InternalsVisibleTo("Project.Editor.Avalonia")]
[assembly: InternalsVisibleTo("Project.Editor.Windows")]
[assembly: InternalsVisibleTo("Project.Roslyn.Windows")]
[assembly: InternalsVisibleTo("Project.Build")]
