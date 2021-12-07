Imports PersonalUtilities.Functions.XML.Base
Namespace API.Twitter
    Friend Module Declarations
        Friend DateProvider As New ADateTime(ADateTime.Formats.BaseDateTime)
        Friend ReadOnly VideoNode As NodeParams() = {New NodeParams("video_info", True, True, True, True, 10)}
        Friend ReadOnly VideoSizeRegEx As New RegexStructure("\d+x(\d+)",,,, 1,,, String.Empty, EDP.ReturnValue)
        Friend ReadOnly UserIdRegEx As New RegexStructure("user_id.:.(\d+)",,,, 1,,, String.Empty, EDP.ReturnValue)
    End Module
End Namespace