﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by coded UI test builder.
//      Version: 16.0.0.0
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace UITest2
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    [GeneratedCode("Coded UITest Builder", "16.0.28315.86")]
    public partial class UIMap
    {
        
        /// <summary>
        /// RecordedMethod1 - Use 'RecordedMethod1Params' to pass parameters into this method.
        /// </summary>
        public void RecordedMethod1()
        {
            #region Variable Declarations
            WinEdit uIBillTextBoxEdit = this.UIForm1Window.UIBillTextBoxWindow.UIBillTextBoxEdit;
            WpfControl uIEnterTotalBillEdit = this.UIForm1Window.UIEnterTotalBillEdit;
            WinWindow uITipBoxWindow = this.UIForm1Window.UIForm1Client.UITipBoxWindow;
            WinEdit uITipBoxEdit = this.UIForm1Window.UITipBoxWindow.UITipBoxEdit;
            WinClient uIForm1Client = this.UIForm1Window.UIForm1Client;
            WinButton uIComputeTipButton = this.UIForm1Window.UIComputeTipWindow.UIComputeTipButton;
            #endregion

            // Launch '%USERPROFILE%\source\repos\u1190338\Labs\Lab6\Lab6\bin\Debug\Lab6.exe'
            ApplicationUnderTest uIForm1Window = ApplicationUnderTest.Launch(this.RecordedMethod1Params.UIForm1WindowExePath, this.RecordedMethod1Params.UIForm1WindowAlternateExePath);

            // Click 'BillTextBox' text box
            Mouse.Click(uIBillTextBoxEdit, new Point(61, 4));

            // Double-Click 'BillTextBox' text box
            Mouse.DoubleClick(uIBillTextBoxEdit, new Point(61, 4));

            // Type '100' in 'Enter Total Bill' text box
            Keyboard.SendKeys(uIEnterTotalBillEdit, this.RecordedMethod1Params.UIEnterTotalBillEditSendKeys, ModifierKeys.None);

            // Click 'TipBox' window
            Mouse.Click(uITipBoxWindow, new Point(32, 1));

            // Type '20' in 'TipBox' text box
            uITipBoxEdit.Text = this.RecordedMethod1Params.UITipBoxEditText;

            // Click 'Form1' client
            Mouse.Click(uIForm1Client, new Point(301, 303));

            // Click 'Compute Tip' button
            Mouse.Click(uIComputeTipButton, new Point(132, 22));
        }
        
        /// <summary>
        /// AssertMethod1 - Use 'AssertMethod1ExpectedValues' to pass parameters into this method.
        /// </summary>
        public void AssertMethod1()
        {
            #region Variable Declarations
            WinEdit uITotalBoxEdit = this.UIForm1Window.UITotalBoxWindow.UITotalBoxEdit;
            #endregion

            // Verify that the 'ControlType' property of 'TotalBox' text box equals '120'
            Assert.AreEqual(this.AssertMethod1ExpectedValues.UITotalBoxEditControlType, uITotalBoxEdit.ControlType.ToString());
        }
        
        /// <summary>
        /// RecordedMethod2 - Use 'RecordedMethod2Params' to pass parameters into this method.
        /// </summary>
        public void RecordedMethod2()
        {
            #region Variable Declarations
            WinEdit uIBillTextBoxEdit = this.UIForm1Window.UIBillTextBoxWindow.UIBillTextBoxEdit;
            WinEdit uITipBoxEdit = this.UIForm1Window.UITipBoxWindow.UITipBoxEdit;
            #endregion

            // Launch '%USERPROFILE%\source\repos\u1190338\Labs\Lab6\Lab6\bin\Debug\Lab6.exe'
            ApplicationUnderTest uIForm1Window = ApplicationUnderTest.Launch(this.RecordedMethod2Params.UIForm1WindowExePath, this.RecordedMethod2Params.UIForm1WindowAlternateExePath);

            // Type '100' in 'BillTextBox' text box
            uIBillTextBoxEdit.Text = this.RecordedMethod2Params.UIBillTextBoxEditText;

            // Type '20' in 'TipBox' text box
            uITipBoxEdit.Text = this.RecordedMethod2Params.UITipBoxEditText;
        }
        
        /// <summary>
        /// AssertMethod2 - Use 'AssertMethod2ExpectedValues' to pass parameters into this method.
        /// </summary>
        public void AssertMethod2()
        {
            #region Variable Declarations
            WinEdit uITotalBoxEdit = this.UIForm1Window.UITotalBoxWindow.UITotalBoxEdit;
            #endregion

            // Verify that the 'ControlType' property of 'TotalBox' text box equals '120'
            Assert.AreEqual(this.AssertMethod2ExpectedValues.UITotalBoxEditControlType, uITotalBoxEdit.ControlType.ToString());
        }
        
        #region Properties
        public virtual RecordedMethod1Params RecordedMethod1Params
        {
            get
            {
                if ((this.mRecordedMethod1Params == null))
                {
                    this.mRecordedMethod1Params = new RecordedMethod1Params();
                }
                return this.mRecordedMethod1Params;
            }
        }
        
        public virtual AssertMethod1ExpectedValues AssertMethod1ExpectedValues
        {
            get
            {
                if ((this.mAssertMethod1ExpectedValues == null))
                {
                    this.mAssertMethod1ExpectedValues = new AssertMethod1ExpectedValues();
                }
                return this.mAssertMethod1ExpectedValues;
            }
        }
        
        public virtual RecordedMethod2Params RecordedMethod2Params
        {
            get
            {
                if ((this.mRecordedMethod2Params == null))
                {
                    this.mRecordedMethod2Params = new RecordedMethod2Params();
                }
                return this.mRecordedMethod2Params;
            }
        }
        
        public virtual AssertMethod2ExpectedValues AssertMethod2ExpectedValues
        {
            get
            {
                if ((this.mAssertMethod2ExpectedValues == null))
                {
                    this.mAssertMethod2ExpectedValues = new AssertMethod2ExpectedValues();
                }
                return this.mAssertMethod2ExpectedValues;
            }
        }
        
        public UIForm1Window UIForm1Window
        {
            get
            {
                if ((this.mUIForm1Window == null))
                {
                    this.mUIForm1Window = new UIForm1Window();
                }
                return this.mUIForm1Window;
            }
        }
        #endregion
        
        #region Fields
        private RecordedMethod1Params mRecordedMethod1Params;
        
        private AssertMethod1ExpectedValues mAssertMethod1ExpectedValues;
        
        private RecordedMethod2Params mRecordedMethod2Params;
        
        private AssertMethod2ExpectedValues mAssertMethod2ExpectedValues;
        
        private UIForm1Window mUIForm1Window;
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'RecordedMethod1'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "16.0.28315.86")]
    public class RecordedMethod1Params
    {
        
        #region Fields
        /// <summary>
        /// Launch '%USERPROFILE%\source\repos\u1190338\Labs\Lab6\Lab6\bin\Debug\Lab6.exe'
        /// </summary>
        public string UIForm1WindowExePath = "C:\\Users\\ben41\\source\\repos\\u1190338\\Labs\\Lab6\\Lab6\\bin\\Debug\\Lab6.exe";
        
        /// <summary>
        /// Launch '%USERPROFILE%\source\repos\u1190338\Labs\Lab6\Lab6\bin\Debug\Lab6.exe'
        /// </summary>
        public string UIForm1WindowAlternateExePath = "%USERPROFILE%\\source\\repos\\u1190338\\Labs\\Lab6\\Lab6\\bin\\Debug\\Lab6.exe";
        
        /// <summary>
        /// Type '100' in 'Enter Total Bill' text box
        /// </summary>
        public string UIEnterTotalBillEditSendKeys = "100";
        
        /// <summary>
        /// Type '20' in 'TipBox' text box
        /// </summary>
        public string UITipBoxEditText = "20";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'AssertMethod1'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "16.0.28315.86")]
    public class AssertMethod1ExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that the 'ControlType' property of 'TotalBox' text box equals '120'
        /// </summary>
        public string UITotalBoxEditControlType = "120";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'RecordedMethod2'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "16.0.28315.86")]
    public class RecordedMethod2Params
    {
        
        #region Fields
        /// <summary>
        /// Launch '%USERPROFILE%\source\repos\u1190338\Labs\Lab6\Lab6\bin\Debug\Lab6.exe'
        /// </summary>
        public string UIForm1WindowExePath = "C:\\Users\\ben41\\source\\repos\\u1190338\\Labs\\Lab6\\Lab6\\bin\\Debug\\Lab6.exe";
        
        /// <summary>
        /// Launch '%USERPROFILE%\source\repos\u1190338\Labs\Lab6\Lab6\bin\Debug\Lab6.exe'
        /// </summary>
        public string UIForm1WindowAlternateExePath = "%USERPROFILE%\\source\\repos\\u1190338\\Labs\\Lab6\\Lab6\\bin\\Debug\\Lab6.exe";
        
        /// <summary>
        /// Type '100' in 'BillTextBox' text box
        /// </summary>
        public string UIBillTextBoxEditText = "100";
        
        /// <summary>
        /// Type '20' in 'TipBox' text box
        /// </summary>
        public string UITipBoxEditText = "20";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'AssertMethod2'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "16.0.28315.86")]
    public class AssertMethod2ExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that the 'ControlType' property of 'TotalBox' text box equals '120'
        /// </summary>
        public string UITotalBoxEditControlType = "120";
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "16.0.28315.86")]
    public class UIForm1Window : WinWindow
    {
        
        public UIForm1Window()
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.Name] = "Form1";
            this.SearchProperties.Add(new PropertyExpression(WinWindow.PropertyNames.ClassName, "WindowsForms10.Window", PropertyExpressionOperator.Contains));
            this.WindowTitles.Add("Form1");
            #endregion
        }
        
        #region Properties
        public UIBillTextBoxWindow UIBillTextBoxWindow
        {
            get
            {
                if ((this.mUIBillTextBoxWindow == null))
                {
                    this.mUIBillTextBoxWindow = new UIBillTextBoxWindow(this);
                }
                return this.mUIBillTextBoxWindow;
            }
        }
        
        public WpfControl UIEnterTotalBillEdit
        {
            get
            {
                if ((this.mUIEnterTotalBillEdit == null))
                {
                    this.mUIEnterTotalBillEdit = new WpfControl(this);
                    #region Search Criteria
                    this.mUIEnterTotalBillEdit.SearchProperties[WpfControl.PropertyNames.ControlType] = "Edit";
                    this.mUIEnterTotalBillEdit.SearchProperties[WpfControl.PropertyNames.Name] = "Enter Total Bill";
                    this.mUIEnterTotalBillEdit.WindowTitles.Add("Form1");
                    #endregion
                }
                return this.mUIEnterTotalBillEdit;
            }
        }
        
        public UIForm1Client UIForm1Client
        {
            get
            {
                if ((this.mUIForm1Client == null))
                {
                    this.mUIForm1Client = new UIForm1Client(this);
                }
                return this.mUIForm1Client;
            }
        }
        
        public UITipBoxWindow UITipBoxWindow
        {
            get
            {
                if ((this.mUITipBoxWindow == null))
                {
                    this.mUITipBoxWindow = new UITipBoxWindow(this);
                }
                return this.mUITipBoxWindow;
            }
        }
        
        public UIComputeTipWindow UIComputeTipWindow
        {
            get
            {
                if ((this.mUIComputeTipWindow == null))
                {
                    this.mUIComputeTipWindow = new UIComputeTipWindow(this);
                }
                return this.mUIComputeTipWindow;
            }
        }
        
        public UITotalBoxWindow UITotalBoxWindow
        {
            get
            {
                if ((this.mUITotalBoxWindow == null))
                {
                    this.mUITotalBoxWindow = new UITotalBoxWindow(this);
                }
                return this.mUITotalBoxWindow;
            }
        }
        #endregion
        
        #region Fields
        private UIBillTextBoxWindow mUIBillTextBoxWindow;
        
        private WpfControl mUIEnterTotalBillEdit;
        
        private UIForm1Client mUIForm1Client;
        
        private UITipBoxWindow mUITipBoxWindow;
        
        private UIComputeTipWindow mUIComputeTipWindow;
        
        private UITotalBoxWindow mUITotalBoxWindow;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "16.0.28315.86")]
    public class UIBillTextBoxWindow : WinWindow
    {
        
        public UIBillTextBoxWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "BillTextBox";
            this.WindowTitles.Add("Form1");
            #endregion
        }
        
        #region Properties
        public WinEdit UIBillTextBoxEdit
        {
            get
            {
                if ((this.mUIBillTextBoxEdit == null))
                {
                    this.mUIBillTextBoxEdit = new WinEdit(this);
                    #region Search Criteria
                    this.mUIBillTextBoxEdit.SearchProperties[WinEdit.PropertyNames.Name] = "Enter Total Bill";
                    this.mUIBillTextBoxEdit.WindowTitles.Add("Form1");
                    #endregion
                }
                return this.mUIBillTextBoxEdit;
            }
        }
        #endregion
        
        #region Fields
        private WinEdit mUIBillTextBoxEdit;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "16.0.28315.86")]
    public class UIForm1Client : WinClient
    {
        
        public UIForm1Client(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinControl.PropertyNames.Name] = "Form1";
            this.WindowTitles.Add("Form1");
            #endregion
        }
        
        #region Properties
        public WinWindow UITipBoxWindow
        {
            get
            {
                if ((this.mUITipBoxWindow == null))
                {
                    this.mUITipBoxWindow = new WinWindow(this);
                    #region Search Criteria
                    this.mUITipBoxWindow.SearchProperties.Add(new PropertyExpression(WinWindow.PropertyNames.ClassName, "WindowsForms10.EDIT", PropertyExpressionOperator.Contains));
                    this.mUITipBoxWindow.SearchProperties[WinWindow.PropertyNames.Instance] = "2";
                    this.mUITipBoxWindow.WindowTitles.Add("Form1");
                    #endregion
                }
                return this.mUITipBoxWindow;
            }
        }
        #endregion
        
        #region Fields
        private WinWindow mUITipBoxWindow;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "16.0.28315.86")]
    public class UITipBoxWindow : WinWindow
    {
        
        public UITipBoxWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "TipBox";
            this.WindowTitles.Add("Form1");
            #endregion
        }
        
        #region Properties
        public WinEdit UITipBoxEdit
        {
            get
            {
                if ((this.mUITipBoxEdit == null))
                {
                    this.mUITipBoxEdit = new WinEdit(this);
                    #region Search Criteria
                    this.mUITipBoxEdit.WindowTitles.Add("Form1");
                    #endregion
                }
                return this.mUITipBoxEdit;
            }
        }
        #endregion
        
        #region Fields
        private WinEdit mUITipBoxEdit;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "16.0.28315.86")]
    public class UIComputeTipWindow : WinWindow
    {
        
        public UIComputeTipWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "ComputeTip";
            this.WindowTitles.Add("Form1");
            #endregion
        }
        
        #region Properties
        public WinButton UIComputeTipButton
        {
            get
            {
                if ((this.mUIComputeTipButton == null))
                {
                    this.mUIComputeTipButton = new WinButton(this);
                    #region Search Criteria
                    this.mUIComputeTipButton.SearchProperties[WinButton.PropertyNames.Name] = "Compute Tip";
                    this.mUIComputeTipButton.WindowTitles.Add("Form1");
                    #endregion
                }
                return this.mUIComputeTipButton;
            }
        }
        #endregion
        
        #region Fields
        private WinButton mUIComputeTipButton;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "16.0.28315.86")]
    public class UITotalBoxWindow : WinWindow
    {
        
        public UITotalBoxWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "TotalBox";
            this.WindowTitles.Add("Form1");
            #endregion
        }
        
        #region Properties
        public WinEdit UITotalBoxEdit
        {
            get
            {
                if ((this.mUITotalBoxEdit == null))
                {
                    this.mUITotalBoxEdit = new WinEdit(this);
                    #region Search Criteria
                    this.mUITotalBoxEdit.SearchProperties[WinEdit.PropertyNames.Name] = "%";
                    this.mUITotalBoxEdit.WindowTitles.Add("Form1");
                    #endregion
                }
                return this.mUITotalBoxEdit;
            }
        }
        #endregion
        
        #region Fields
        private WinEdit mUITotalBoxEdit;
        #endregion
    }
}
