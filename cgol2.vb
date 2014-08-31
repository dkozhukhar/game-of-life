Imports System.Windows.Forms
Imports System.Collections.CollectionBase 'used in buttonarray
Imports System.Resources 'image load
Imports System.Timers
imports System.Drawing
imports System.Drawing.Imaging


Public Class Test
    Inherits System.Windows.Forms.Form
    
    dim Xsize as integer = 150
    dim Ysize as integer = 55
	dim myRad as integer = 6
    dim ratio as integer = 10
	
    private b as New Button()
    private c as New Button()
    private d as New Button()
	private l as new label()
    
    Private Shared aTimer As System.Timers.Timer
    
    'Dim MyControlArray as ButtonArray
    
    'public mybuttarray(Xsize,Ysize) as button
	public action as boolean = false	
	public tick as integer

	Private pictureBox1 As New PictureBox()
	public myBitmap = New Bitmap(Xsize+1, Ysize+1)
	
	public field0 = new Byte(Xsize, Ysize) {}
	
	
	'''''''''''''''''''''''''''''''
	''''''''''''''''''''''''''''''' 
    
    Public Sub New()
        MyBase.New()
        MyBase.Topmost = True
        MyBase.Text = "Это Заголовок формы"
        MyBase.Size = new System.Drawing.Size(900,700)
        
       ' MyControlArray = New ButtonArray(Me)
       ' MyControlArray.AddNewButton()
       ' MyControlArray(0).BackColor = System.Drawing.Color.Red
        
        b.Dock = DockStyle.Top
        b.Text = "Это Кнопка b; Очистить поле"
        addhandler b.Click, addressof b_click    

        c.Dock = DockStyle.Top
        c.Text = "Это Кнопка c; Заполнить случайным образцом"
        addhandler c.Click, addressof c_click         

        d.Dock = DockStyle.Top
        d.Text = "Это Кнопка d; Старт/Пауза"
        addhandler d.Click, addressof d_click      
        
		l.dock = DockStyle.Top
        l.Text = "Это Метка l; Счеткик хода"
		
		MyBase.Controls.Add(l)
        MyBase.Controls.Add(d)
        MyBase.Controls.Add(c)
        MyBase.Controls.Add(b)
        
         
		
        MyBase.Show()                                
            
		'b.PerformClick()  
        'c.PerformClick()  
		
		
		
        call settimer
		'd.PerformClick()  
		
		
		
		'pictureBox1.Size = New Size(Xsize+1, Ysize+1)	
		pictureBox1.Size = New Size(800	 , 400)	
		PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
		pictureBox1.SizeMode = PictureBoxSizeMode.zoom
		PictureBox1.BorderStyle = BorderStyle.Fixed3D
		
		'http://msdn.microsoft.com/ru-ru/library/system.windows.forms.picturebox.picturebox(v=vs.110).aspx
		PictureBox1.Location = New System.Drawing.Point(10, 100)	
		
		MyBase.Controls.Add(pictureBox1)		
		


		
    End sub

    ''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''
	
	'<MTAThread> _
	<STAThread()> _
    Public Shared Sub Main()
        'Dim T as New Test
        'T = New Test()
        System.Windows.Forms.Application.Run(New Test())
    End Sub
    
	''''''''''''''''''''''''''''''''''''
	''''''''''''''''''''''''''''''''''''	
	'''''''''''''''''''''''''''''''''''
	'''''''''''''''''''''''''''''''''''

	public sub clearfield()
	
		dim i,j as integer
		
		for i = 0 to Xsize
			for j = 0 to Ysize
				field0(i,j) = 0
			next j
		next i
	
	end sub 'clearfield
	
	'''''''''''''''''''''''
	
	public sub randomfield()
	
		dim i,j as integer
		
		for i = 0 to Xsize
			for j = 0 to Ysize
				field0(i,j) = CInt(Math.Floor(2*Rnd())) 'randomValue(0, 1)
			next j
		next i
	
	end sub 'randomfield
	
	'''''''''''''''''''''''
	
	public sub stepfield()
	
		dim i,j as integer
		dim r,s as integer
		dim count as integer
		dim field1 = new Byte(Xsize, Ysize) {}
		
		
		for i = 0 to Xsize
			for j = 0 to Ysize

				count = 0 - field0(i,j)
				for r = Math.Max(0, i - 1) to Math.Min(Xsize, i + 1)
					for s = Math.Max(0, j - 1) to Math.Min(Ysize, j + 1)
					
						count += field0(r,s)
					
					next s
				next r
				
				if field0(i,j) = 0 then
					field1(i,j) = if(count=3,1,0)
					else 
					field1(i,j) = if(count=2 orelse count=3,1,0)
				end if
								
			next j
		next i
			
		field0 = field1
	
	end sub 'stepfield
	'''''''''''''''''''''''
	'''''''''''''''''''''''
	public sub tickerupdate
	
		tick += 1
		l.text = String.Format("Ход №{0}",tick)
	
	end sub 'tickerupdate
	
	public sub tickerrefresh
	
		tick = 0
		l.text = String.Format("Счетчик ходов сброшен в {0}", tick)
	
	end sub 'tickerrefresh
	
	'''''''''''''''''''''''
	'''''''''''''''''''''''
	
	public sub refreshgraph()
	
		Dim Xcount, Ycount As Integer
		For Xcount = 0 To myBitmap.Width - 1
			For Ycount = 0 To myBitmap.Height - 1
			
				if field0(Xcount, Ycount)=0 then			
					myBitmap.SetPixel(Xcount, Ycount, Color.Black) 
					else
					myBitmap.SetPixel(Xcount, Ycount, Color.White)
				end if
				
			Next Ycount
		Next Xcount		
		 	
		pictureBox1.Image = myBitmap
		me.pictureBox1.Invalidate
	
	end sub 'refreshgraph
	
	''''''''''''''''''''''''''''''''''''
	'''''''''''''''''''''''''''''''''''
	
	
    public sub settimer
        ' Create a timer with a ten second interval.
        aTimer = New System.Timers.Timer(10)

        ' Hook up the Elapsed event for the timer. 
        AddHandler aTimer.Elapsed, AddressOf OnTimedEvent

        ' Set the Interval to 2 seconds (2000 milliseconds).
        'aTimer.Interval = 200
        
        aTimer.SynchronizingObject = me
    
    end sub

	
    ''''''''''''''''''''''''''''''''''''
  
    ''''''''''''''''''''''''''''''''''''
   
    public function randomValue(byref lowerbound as integer,byref upperbound as integer) as integer
        randomValue = CInt(Math.Floor((upperbound - lowerbound + 1) * Rnd())) + lowerbound
    end function    
    

	
	''''''''''''''''''''''''''''''''''''
    
    public sub b_click(ByVal sender as object, byval e as eventargs)
        console.writeline("b_click executed" & vbCrlf & "on object : " & sender.tostring() & vbCrlf & vbTab  & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
		
		call clearfield
		call refreshgraph
		
		call tickerrefresh
		
    end sub
    
	''''''''''''''''''''''''''''''''''''
	
    public sub c_click(ByVal sender as object, byval e as eventargs)
       console.writeline("c_click executed" & vbCrlf & "on object : " & sender.tostring() & vbCrlf & vbtab  & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))       
	   	
		
		call randomfield
		call refreshgraph
		
		call tickerrefresh
		
    end sub
	
'''''
	
	''''''''''''''''''''''''''''''''''''
    
    public sub d_click(ByVal sender as object, byval e as eventargs)
       console.writeline("d_click executed" & vbCrlf & "on object : " & sender.tostring() & vbCrlf & vbtab  & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))       
      
		call flipaction
		
		call fliptimer
       
    end sub    
    ''''''''''''''''''''''''''''''''''''
    
	
	
	public sub fliptimer

		   if aTimer.Enabled = True then
				aTimer.Enabled = False
		   else 
				aTimer.Enabled = True     
		   end if

	end sub 'fliptimer	
	
	'''''''''''''
	
	public sub flipaction

		action = not action

	end sub 'flipaction		
	
	
	
    ' Specify what you want to happen when the Elapsed event is  
    ' raised. 
    
    public Sub OnTimedEvent(source As Object, e As ElapsedEventArgs)
        Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime)
		
		call fliptimer
		call stepfield
		call refreshgraph	
		call tickerupdate
		call fliptimer
		
    End Sub 
    

	public sub actionevent
	
		dim t as integer
	
		for t = 0 to 10
	
			call stepfield			
			call refreshgraph	
		
		next t
	
	end sub 'actionevent
	
	
End Class 'test


''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
