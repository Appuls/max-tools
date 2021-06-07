systemEnvironment = dotNetClass "System.Environment"
specialFolder = dotNetClass "System.Environment+specialFolder"
appData = systemEnvironment.getfolderPath specialFolder.localApplicationData
assemblyPath = appData + @"\MaxTools\__YEAR__\MaxToolsLib.dll"
dotNet.loadAssembly assemblyPath
bootstrap = dotNetClass "MaxToolsLib.Bootstrap"

fn maxTools_openDialog = (
    bootstrap.OpenDialog()
)

fn maxTools_handleSelectionChanged = (
    bootstrap.HandleSelectionChanged()
)

fn maxTools_observeSelectionChanged enabled = (
    if enabled then (
        callbacks.addScript #selectionSetChanged "maxTools_handleSelectionChanged()" id:#maxTools_selection_observer
    ) else (
        callbacks.removeScripts id:#maxTools_selection_observer
    )
)

macroCode = "macroScript MaxTools_Open
category: \"MaxTools\"
buttonText: \"Open\"
tooltip: \"Open MaxTools\"
(
    maxTools_openDialog ()
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
