systemEnvironment = dotNetClass "System.Environment"
specialFolder = dotNetClass "System.Environment+specialFolder"
appData = systemEnvironment.getfolderPath specialFolder.localApplicationData
assemblyPath = appData + @"\MaxTools\__YEAR__\UserPropsEditor.exe"
dotNet.loadAssembly assemblyPath
app = dotNetClass "UserPropsEditor.App"

fn openDialog = (
    app.Create()
)

macroCode = "macroScript MaxTools_Open
category: \"MaxTools\"
buttonText: \"Open\"
tooltip: \"Open MaxTools\"
(
    openDialog ()
)
"

if menuMan.registerMenuContext 0x572418f3 then
(
    local topBar = menuMan.getMainMenuBar()
    local maxToolsMenu = menuMan.createMenu "MaxTools"
    local tmp = menuMan.createActionItem "MaxTools_Open" "MaxTools"
    maxToolsMenu.addItem tmp -1

    local subMenuItem = menuMan.createSubMenuItem "MaxTools" maxToolsMenu
    local subMenuIndex = topBar.numItems()

    topBar.addItem subMenuItem subMenuIndex

    menuMan.updateMenuBar()
)
