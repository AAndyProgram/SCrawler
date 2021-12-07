Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Tools.WEB
Namespace Editors
    Friend Class TwitterEditorForm : Implements IOkCancelToolbar
        Private ReadOnly MyDefs As DefaultFormProps(Of FieldsChecker)
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormProps(Of FieldsChecker)
        End Sub
        Private Sub TwitterEditorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize(Me, Settings.Design, True)
                    .AddOkCancelToolbar()
                    .DelegateClosingChecker()
                    With Settings.Site(Sites.Twitter)
                        TXT_PATH.Text = .Path
                        With .Responser
                            If .Cookies Is Nothing Then .Cookies = New CookieKeeper(.CookiesDomain)
                            SetCookieText()
                            TXT_TOKEN.Text = .Headers(API.Base.SiteSettings.Header_Twitter_Token)
                            TXT_AUTH.Text = .Headers(API.Base.SiteSettings.Header_Twitter_Authorization)
                        End With
                    End With
                    .MyFieldsChecker = New FieldsChecker
                    With .MyFieldsChecker
                        .AddControl(Of String)(TXT_TOKEN, TXT_TOKEN.CaptionText)
                        .AddControl(Of String)(TXT_AUTH, TXT_AUTH.CaptionText)
                        .EndLoaderOperations()
                    End With
                    TextBoxExtended.SetFalseDetector(Me, True, AddressOf .Detector)
                    .EndLoaderOperations()
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub ToolbarBttOK() Implements IOkCancelToolbar.ToolbarBttOK
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                With Settings.Site(Sites.Twitter)
                    If TXT_PATH.IsEmptyString Then .Path = Nothing Else .Path = TXT_PATH.Text
                    With .Responser
                        .Headers(API.Base.SiteSettings.Header_Twitter_Token) = TXT_TOKEN.Text
                        .Headers(API.Base.SiteSettings.Header_Twitter_Authorization) = TXT_AUTH.Text
                    End With
                    .Update()
                End With
                MyDefs.CloseForm()
            End If
        End Sub
        Private Sub ToolbarBttCancel() Implements IOkCancelToolbar.ToolbarBttCancel
            MyDefs.CloseForm(DialogResult.Cancel)
        End Sub
        Private Sub TXT_PATH_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_PATH.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Open Then
                Dim f As SFile = SFile.SelectPath(Settings.Site(Sites.Twitter).Path)
                If Not f.IsEmptyString Then TXT_PATH.Text = f
            End If
        End Sub
        Private Sub TXT_COOKIES_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_COOKIES.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Edit Then
                Using f As New CookieListForm(Settings.Site(Sites.Twitter).Responser.Cookies) : f.ShowDialog() : End Using
                SetCookieText()
            End If
        End Sub
        Private Sub TXT_COOKIES_ActionOnButtonClearClick() Handles TXT_COOKIES.ActionOnButtonClearClick
            With Settings.Site(Sites.Twitter).Responser
                If Not .Cookies Is Nothing Then .Cookies.Dispose()
                .Cookies = New CookieKeeper(.CookiesDomain)
            End With
            SetCookieText()
        End Sub
        Private Sub SetCookieText()
            Dim c% = 0
            With Settings.Site(Sites.Twitter).Responser
                If Not .Cookies Is Nothing Then c = .Cookies.Count
            End With
            TXT_COOKIES.Text = $"{c} cookies"
        End Sub
    End Class
End Namespace