REM [%1 = $(TargetDir), %2 = $(AssemblyName), %3 = $(ProjectDir)]
xcopy /Y %1%2.dll %3..\..\Library\
xcopy /Y %1%2.dll %3..\..\Unity\Mimi.ObjectableEditorGUI.Package\Assets\Plugins\Mimi.ObjectableEditorGUI\Editor\Library\