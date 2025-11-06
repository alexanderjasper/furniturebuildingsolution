' New CamBam VBScript

' Lead in move should be set manually since it is not included in the script yet.
' Pocket: Type=Spiral, Spiral Angle=0, Tangent Radius=0, Lead Move Feedrate=0
' Profile: Type=Spiral, Spiral Angle=45, Tangent Radius=0, Lead Move Feedrate=0

sub main
	dim doc = CamBamUI.MainUI.ActiveView.CADFile

	dim plateThickness = 16.4
	dim depth = -0.25
	dim db
	dim it

	If doc.Layers("RearHoles") IsNot Nothing Then
		dim drillRear = new CamBam.CAM.MOPPocket(doc,doc.Layers("RearHoles").Entities.ToArray())
	
		db = drillRear.ToolDiameter
		it = drillRear.SpindleSpeed
	
		db.SetValue(4.0)
		drillRear.ToolDiameter = db

		it.SetValue(1)
		drillRear.ToolNumber = it
	
		db.SetValue(1000.0)
		drillRear.PlungeFeedrate = db
	
		db.SetValue(3000.0)
		drillRear.CutFeedrate = db
	
		it.SetValue(18000)
		drillRear.SpindleSpeed = it
	
		db.SetValue(12.0)
		drillRear.DepthIncrement = db
	
		db.SetValue(plateThickness - 12.0)
		drillRear.TargetDepth = db

		db.SetValue(plateThickness + 15.0)
		drillRear.ClearancePlane = db

		db.SetValue(plateThickness)
		drillRear.StockSurface = db

		
		CamBamUI.MainUI.InsertMOP(drillRear)
	End If

	If doc.Layers("Sheets") IsNot Nothing Then
		dim profile = new CamBam.CAM.MOPProfile(doc,doc.Layers("Sheets").Entities.ToArray())

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
	
		db.SetValue(depth )
		profile.TargetDepth = db

		db.SetValue(plateThickness + 15.0)
		profile.ClearancePlane = db

		db.SetValue(plateThickness)
		profile.StockSurface = db

		CamBamUI.MainUI.InsertMOP(profile)
	End If
end sub