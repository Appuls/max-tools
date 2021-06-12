(
    global maxTools_bootstrap
    
    global maxTools_openDialog
    global maxTools_handleSelectionChanged
    global maxTools_observeSelectionChanged
    
    fn maxTools_initialize = (
        local thisDir = (pathConfig.removePathLeaf (getSourceFileName()))
        local assemblyPath = thisDir + @"\max-tools\MaxToolsLib.dll"
        dotNet.loadAssembly assemblyPath
        maxTools_bootstrap = dotNetClass "MaxToolsLib.Bootstrap"
            
        fn maxTools_openDialog = (
            maxTools_bootstrap.OpenDialog()
        )

        fn maxTools_handleSelectionChanged = (
            maxTools_bootstrap.HandleSelectionChanged()
        )

        fn maxTools_observeSelectionChanged enabled = (
            if enabled then (
                callbacks.addScript #selectionSetChanged "maxTools_handleSelectionChanged()" id:#maxTools_selection_observer
            ) else (
                callbacks.removeScripts id:#maxTools_selection_observer
            )
        )
        
        print ("max-tools initialized: " + (maxTools_bootstrap as string))
    )
    
    if maxTools_bootstrap == undefined then maxTools_initialize()
    maxTools_openDialog()
    OK
)
