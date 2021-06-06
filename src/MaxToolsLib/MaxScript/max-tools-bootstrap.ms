systemEnvironment = dotNetClass "System.Environment"
specialFolder = dotNetClass "System.Environment+specialFolder"
appData = systemEnvironment.getfolderPath specialFolder.localApplicationData
assemblyPath = appData + @"\MaxTools\__YEAR__\MaxToolsLib.dll"
dotNet.loadAssembly assemblyPath
bootstrap = dotNetClass "MaxToolsLib.Bootstrap"

fn openDialog = (
    bootstrap.OpenDialog()
)

macroCode = "macroScript MaxTools_Open
category: \"MaxTools\"
buttonText: \"Open\"
tooltip: \"Open MaxTools\"
(
    openDialog ()
)
"

execute macroCode

if menuMan.registerMenuContext 0xf1c22777 then
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
