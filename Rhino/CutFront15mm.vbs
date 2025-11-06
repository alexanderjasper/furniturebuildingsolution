' New CamBam VBScript

' Lead in move should be set manually since it is not included in the script yet.
' Pocket: Type=Spiral, Spiral Angle=0, Tangent Radius=0, Lead Move Feedrate=0
' Profile: Type=Spiral, Spiral Angle=45, Tangent Radius=0, Lead Move Feedrate=0
' !!Must first manually convert to polyline and join - Ctrl+A Ctrl+P Ctrl+J !!To be fixed

sub main
	dim doc = CamBamUI.MainUI.ActiveView.CADFile
	Console.WriteLine("Hello")
	
	

	CamBamUI.MainUI.ActiveView.SelectObjects(doc.Layers("Plates").Entities.ToArray())
	PolylineUtils.JoinPolyLines(CamBamUI.MainUI.ActiveView,0.001)
	doc.Update()

	dim plateThickness = 16.4
	dim depth = -0.25
	dim indentDepth = 0.5
	dim firstPassDepth = 2.0
	dim drillTip = 1.0
	dim db
	dim it



	If doc.Layers("Holes") IsNot Nothing Then
		dim drill = new CamBam.CAM.MOPPocket(doc,doc.Layers("Holes").Entities.ToArray())
	
		db = drill.ToolDiameter
		it = drill.SpindleSpeed
	
		db.SetValue(4.0)
		drill.ToolDiameter = db

		it.SetValue(1)
		drill.ToolNumber = it
	
		db.SetValue(1000.0)
		drill.PlungeFeedrate = db
	
		db.SetValue(3000.0)
		drill.CutFeedrate = db
	
		it.SetValue(18000)
		drill.SpindleSpeed = it
	
		db.SetValue(12.0)
		drill.DepthIncrement = db
	
		db.SetValue(plateThickness - 12.0)
		drill.TargetDepth = db

		db.SetValue(plateThickness + 15.0)
		drill.ClearancePlane = db

		db.SetValue(plateThickness)
		drill.StockSurface = db

		
		CamBamUI.MainUI.InsertMOP(drill)
	End If

	If doc.Layers("ThroughHoles") IsNot Nothing Then
		dim drillThrough = new CamBam.CAM.MOPDrill(doc,doc.Layers("ThroughHoles").Entities.ToArray())
	
		db = drillThrough.ToolDiameter
		it = drillThrough.SpindleSpeed
	
		db.SetValue(4.999)
		drillThrough.ToolDiameter = db

		it.SetValue(3)
		drillThrough.ToolNumber = it
	
		db.SetValue(900.0)
		drillThrough.PlungeFeedrate = db
		
		it.SetValue(2400)
		drillThrough.SpindleSpeed = it

		db.SetValue(plateThickness - depth + drillTip)
		drillThrough.DepthIncrement = db
		
		db.SetValue(depth-drillTip)
		drillThrough.TargetDepth = db

		db.SetValue(plateThickness + 15.0)
		drillThrough.ClearancePlane = db

		db.SetValue(plateThickness)
		drillThrough.StockSurface = db

		
		CamBamUI.MainUI.InsertMOP(drillThrough)
	End If


	If doc.Layers("Plates") IsNot Nothing Then
		dim profile = new CamBam.CAM.MOPProfile(doc,doc.Layers("Plates").Entities.ToArray())

		db = profile.ToolDiameter
		it = profile.SpindleSpeed
	
		db.SetValue(4.0)
		profile.ToolDiameter = db

		it.SetValue(1)
		profile.ToolNumber = it
	
		db.SetValue(1000.0)
		profile.PlungeFeedrate = db
	
		db.SetValue(4000.0)
		profile.CutFeedrate = db
	
		it.SetValue(18000)
		profile.SpindleSpeed = it
	
		db.SetValue(plateThickness - depth)
		profile.DepthIncrement = db
	
		db.SetValue(depth)
		profile.TargetDepth = db

		db.SetValue(plateThickness + 15.0)
		profile.ClearancePlane = db

		db.SetValue(plateThickness)
		profile.StockSurface = db

		CamBamUI.MainUI.InsertMOP(profile)

	End If

	If doc.Layers("Sockets") IsNot Nothing Then
		CamBamUI.MainUI.ActiveView.SelectObjects(doc.Layers("Sockets").Entities.ToArray())
			PolylineUtils.JoinPolyLines(CamBamUI.MainUI.ActiveView,0.001)
			doc.Update()

		dim drill = new CamBam.CAM.MOPDrill(doc,doc.Layers("Sockets").Entities.ToArray())

		db = drill.ToolDiameter
		it = drill.SpindleSpeed
	
		db.SetValue(1.999)
		drill.ToolDiameter = db

		it.SetValue(2)
		drill.ToolNumber = it
		
		db.SetValue(900.0)
		drill.PlungeFeedrate = db
		
		it.SetValue(2400)
		drill.SpindleSpeed = it
	
		db.SetValue(11.8)
		drill.DepthIncrement = db
	
		db.SetValue(plateThickness - 11.8)
		drill.TargetDepth = db

		db.SetValue(plateThickness + 15.0)
		drill.ClearancePlane = db

		db.SetValue(plateThickness)
		drill.StockSurface = db

		CamBamUI.MainUI.InsertMOP(drill)
	End If


	If doc.Layers("Indents") IsNot Nothing Then
		dim indent	 = new CamBam.CAM.MOPPocket(doc,doc.Layers("Indents").Entities.ToArray())

		db = indent.ToolDiameter
		it = indent.SpindleSpeed
	
		db.SetValue(4.0)
		indent.ToolDiameter = db

		it.SetValue(1)
		indent.ToolNumber = it
	
		db.SetValue(1000.0)
		indent.PlungeFeedrate = db
	
		db.SetValue(3000.0)
		indent.CutFeedrate = db
	
		it.SetValue(18000)
		indent.SpindleSpeed = it
	
		db.SetValue(0.0)
		indent.DepthIncrement = db
	
		db.SetValue(plateThickness - 11.0 - indentDepth)
		indent.TargetDepth = db

		db.SetValue(plateThickness + 15.0)
		indent.ClearancePlane = db

		db.SetValue(plateThickness)
		indent.StockSurface = db
	
		CamBamUI.MainUI.InsertMOP(indent)
	End If
end sub