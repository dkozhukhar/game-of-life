Imports System.Windows.Forms
Imports System.Collections.CollectionBase 'used in buttonarray
Imports System.Resources 'image load
Imports System.Timers
imports System.Drawing
imports System.Drawing.Imaging


Public Class Test
    Inherits System.Windows.Forms.Form
    
    dim Xsize as integer = 96
    dim Ysize as integer = 96
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
	
	public field0 = new Boolean(Xsize, Ysize) {}
    public field1 = new Boolean(Xsize, Ysize) {}
    public count0 = new Integer(Xsize, Ysize) {}
    public count1 = new Integer(Xsize, Ysize) {}
	
    Dim elapsedtime as System.TimeSpan
	Private _elapseStartTime, _lastClick As DateTime
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
		'   http://msdn.microsoft.com/en-us/library/system.windows.forms.pictureboxsizemode(v=vs.110).aspx
		PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
		pictureBox1.SizeMode = PictureBoxSizeMode.zoom
		PictureBox1.BorderStyle = BorderStyle.Fixed3D
		
		'http://msdn.microsoft.com/ru-ru/library/system.windows.forms.picturebox.picturebox(v=vs.110).aspx
		PictureBox1.Location = New System.Drawing.Point(10, 100)	
		
		MyBase.Controls.Add(pictureBox1)		
		

		call actionevent
		
    End sub

    ''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''
	
	'http://msdn.microsoft.com/en-us/library/ms182351.aspx
	<STAThread()> _
    Public Shared Sub ItsNotMain()
        Dim T as Test
        T = New Test()
        System.Windows.Forms.Application.Run (T)'(New Test())
    End Sub
    
	''''''''''''''''''''''''''''''''''''
	''''''''''''''''''''''''''''''''''''	
	'''''''''''''''''''''''''''''''''''
	'''''''''''''''''''''''''''''''''''

	public sub clearfield()
	
		dim i,j as integer
		
		for i = 0 to Xsize
			for j = 0 to Ysize
				field0(i,j) = False
                count0(i,j) = 0 
                field1(i,j) = False
                count1(i,j) = 0 
			next j
		next i
        
    
    
    
	end sub 'clearfield
	
	'''''''''''''''''''''''
    
	'supposed to use after clearfield
	public sub randomfield()
	
		dim i,j as integer
		
        for i = 0 to Xsize
			field0(i,0) = false
			field0(i,Ysize) = false
		next i
	
		for j = 0 to Ysize
			field0(0,j) = false
			field0(Xsize,j) = false
		next j
        
        
		for i = 1 to Xsize -1
			for j = 1 to Ysize -1
				if CBool(Math.Floor(2*Rnd())) then 'randomValue(0, 1)
                    
                    call set_cell(i,j)
                    
                    else
                    
                    call fade_cell(i,j)
                    
                end if
			next j
		next i

    field0 = field1
    count0 = count1

	end sub 'randomfield
	
	'''''''''''''''''''''''
	public sub fade_cell(i,j)
        
        field1(i,j) = false
        
        myBitmap.SetPixel(i, j, Color.White)   
    
    end sub 'fade cell
    
    public sub set_cell(i,j)

        field1(i,j) = true
        
        myBitmap.SetPixel(i, j, Color.Black)
        
        count1(i-1,j-1) +=1
        count1(i-1,j)  +=1
        count1(i-1,j+1)  +=1
        count1(i,j-1)  +=1
        
        count1(i,j+1)  +=1
        count1(i+1,j-1)  +=1
        count1(i+1,j)  +=1
        count1(i+1,j+1)  +=1

#if false then        
        if i=1 then
            count1(Xsize,j-1) +=1
            count1(Xsize,j)  +=1
            count1(Xsize,j+1)  +=1            
        end if
        if i=Xsize-1 then
            count1(0,j-1) +=1
            count1(0,j)  +=1
            count1(0,j+1)  +=1            
        end if
        if j=1 then
            count1(i-1,Ysize) +=1
            count1(i,Ysize)  +=1
            count1(i+1,Ysize)  +=1         
        end if
        if j=Ysize-1 then
            count1(i-1,0)  +=1
            count1(i,0)  +=1
            count1(i+1,0)  +=1      
        end if        
#end if        
        
    end sub 'set cell
    
    public sub del_cell(i,j)

        field1(i,j) = false
        
        myBitmap.SetPixel(i, j, Color.White)
        
        count1(i-1,j-1) -=1
        count1(i-1,j)  -=1
        count1(i-1,j+1)  -=1
        count1(i,j-1)  -=1
        
        count1(i,j+1)  -=1
        count1(i+1,j-1)  -=1
        count1(i+1,j)  -=1
        count1(i+1,j+1)  -=1


#if false then           
        if i=1 then
            
            count1(Xsize,j-1) -=1
            count1(Xsize,j)  -=1
            count1(Xsize,j+1)  -=1            
        end if
        if i=Xsize-1 then
            count1(0,j-1) -=1
            count1(0,j)  -=1
            count1(0,j+1)  -=1            
        end if
        if j=1 then
            count1(i-1,Ysize) -=1
            count1(i,Ysize)  -=1
            count1(i+1,Ysize)  -=1         
        end if
        if j=Ysize-1 then
            count1(i-1,0)  -=1
            count1(i,0)  -=1
            count1(i+1,0)  -=1      
        end if           
#end if  
    
    end sub 'del_cell cell    
    
	public sub stepfield()
	
		dim i,j as integer
		'dim r,s as integer
		
        field1 = new Boolean(Xsize, Ysize) {}
        count1 = new Integer(Xsize, Ysize) {} 
        
        'замкнутый мир
		for i = 0 to Xsize
			field0(i,0) = field0(i,Ysize-1)
            count0(i,0) = count0(i,Ysize-1)
            
			field0(i,Ysize) = field0(i,1)
            count0(i,Ysize) = count0(i,1)
		next i
        
		for j = 0 to Ysize
			field0(0,j) = field0(Xsize-1,j)
            count0(0,j) = field0(Xsize-1,j)
            
			field0(Xsize,j) = field0(1,j)
            count0(Xsize,j) = field0(1,j)
		next j        
        
        
       
        ' копируем новый массив
		for i = 0 to Xsize
			for j = 0 to Ysize
        
                field1(i,j) = field0(i,j)
                count1(i,j) = count0(i,j)
			next j
		next i        

        
		' расчет нового шага
		for i = 1 to Xsize-1
			for j = 1 to Ysize-1
                
                    if not field0(i,j) then        
                        if count0(i,j) = 3 then call set_cell(i,j)
                    else 'field0 = true
                        if not count0(i,j) = 2 andalso not count0(i,j) = 3 then                        
                            call del_cell(i,j)
                        end if                    
                    end if
                
			next j
		next i
			
		field0 = field1
        count0 = count1
	
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
	
		 	
		pictureBox1.Image = myBitmap
		'me.pictureBox1.Invalidate
	
	end sub 'refreshgraph
	
	''''''''''''''''''''''''
	' уже не используется
	public sub refreshgraph1()  
	
		Dim Xcount, Ycount As Integer
		For Xcount = 0 To myBitmap.Width - 1
			For Ycount = 0 To myBitmap.Height - 1
			
				if field0(Xcount, Ycount) then			
					myBitmap.SetPixel(Xcount, Ycount, Color.Black) 
					else
					myBitmap.SetPixel(Xcount, Ycount, Color.White)
				end if
				
			Next Ycount
		Next Xcount		
		 	
		pictureBox1.Image = myBitmap
		me.pictureBox1.Invalidate
	
	end sub 'refreshgraph1
	
	''''''''''''''''''''''''''''''''''''
	'''''''''''''''''''''''''''''''''''
	
	
    public sub settimer
        ' Create a timer with a ten second interval.
        aTimer = New System.Timers.Timer(1)

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
	   	
		call clearfield
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
		
		'http://msdn.microsoft.com/en-us/library/system.windows.forms.application.doevents(v=vs.110).aspx
		Application.DoEvents()
		call fliptimer
		
    End Sub 
    

	public sub actionevent
	
		dim t as integer
	
        call clearfield
		call randomfield
		call tickerrefresh
	
        dim cycles as integer = 200
    
        _elapseStartTime = DateTime.Now
        
		for t = 1 to cycles
	
			call stepfield			
			call refreshgraph	
			call tickerupdate
			Application.DoEvents()
			
			'System.Threading.Thread.Sleep(100)
			
		next t
	
        elapsedtime = DateTime.Now.Subtract(_elapseStartTime)
        msgbox(String.Format("Время на {0} циклов затрачено {1}; На 1 цикл затрачено {2} ms",cycles,elapsedtime,elapsedtime.TotalMilliseconds/cycles))
        
        
		'MyBase.Close()
		'System.Windows.Forms.Application.Exit()
		
	end sub 'actionevent
	
	
End Class 'test


''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

module xxx

	'http://msdn.microsoft.com/en-us/library/ms182351.aspx
	<STAThread()> _
    Public  Sub Main()
        Dim T as Test
        T = New Test()
        System.Windows.Forms.Application.Run (T)'(New Test())
		

    End Sub

end module
