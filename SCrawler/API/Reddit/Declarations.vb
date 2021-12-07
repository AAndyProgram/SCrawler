Imports PersonalUtilities.Functions.XML.Base
Namespace API.Reddit
    Friend Module Declarations
        Friend ReadOnly JsonNodesJson() As NodeParams = {New NodeParams("posts", True, True, True, True, 3)}
        Friend ReadOnly ChannelJsonNodes() As NodeParams = {New NodeParams("data", True, True, True, True, 1),
                                                            New NodeParams("children", True, True, True)}
        Friend ReadOnly UrlBasePattern As New RegexStructure("(?<=/)([^/]+?\.[\w]{3,4})(?=(\?|\Z))", True, False)
        Friend ReadOnly VideoRegEx As New RegexStructure("http.{0,1}://[^" & Chr(34) & "]+?mp4", True, False)
        Friend ReadOnly DateProvider As New JsonDate
        Friend ReadOnly DateProviderChannel As New JsonDateChannel
        Private ReadOnly EUR_PROVIDER As New ANumbers(ANumbers.Modes.EUR)
        Friend Class JsonDate : Implements ICustomProvider
            ''' <inheritdoc cref="ADateTime.ParseUnicodeJS(Object, Object, ErrorsDescriber)"/>
            Friend Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                    Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                Return ADateTime.ParseUnicodeJS(Value, NothingArg, e)
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException("GetFormat does not available in this context")
            End Function
        End Class
        Friend Class JsonDateChannel : Implements ICustomProvider
            ''' <inheritdoc cref="ADateTime.ParseUnicodeJS(Object, Object, ErrorsDescriber)"/>
            Friend Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                    Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                Return ADateTime.ParseUnicode(AConvert(Of Integer)(Value, EUR_PROVIDER, Value), NothingArg, e)
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException("GetFormat does not available in this context")
            End Function
        End Class
    End Module
End Namespace