' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.PornHub
    Friend Module Declarations
#Region "Converters"
        Private ReadOnly UnicodeHexConverter As Func(Of String, String) = Function(Input) SymbolsConverter.UnicodeHex.Decode(Input, EDP.ReturnValue)
#End Region
#Region "Declarations video"
        Friend ReadOnly RegexVideo_FlashVarsBlocks As RParams = RParams.DM("(?<=(flashvars_\['[nN]ext[vV]ideo'\]|flashvars_\d+[^ ]+? = media_\d+?);[\r\n]*?)(.+?)(?=;flashvars_\d+?)",
                                                                           0, RegexReturn.List, EDP.ReturnValue)
        'TODELETE: PornHub old 'RegexVideo_FlashVarsBlock' declaration
        'Friend ReadOnly RegexVideo_FlashVarsBlock As RParams = RParams.DM("(?<=flashvars_\['[nN]ext[vV]ideo'\];[\r\n]*?)(.+?)(?=;flashvars_\d+?)", 0, EDP.ReturnValue)
        Friend ReadOnly RegexVideo_FlashVars_Vars As RParams = RParams.DM("var ([\w\d]{10,})=("".+?)(?=(;|\Z))", 0, RegexReturn.List)
        Friend ReadOnly RegexVideo_FlashVars_Compiler As RParams = RParams.DM("(?<=\*/)([\w\d\S]{10,})", 0, RegexReturn.List)
        Friend ReadOnly RegexVideo_FlashVars_UrlResolution As RParams = RParams.DMS("/(\d+)[^/]+\.mp4", 1, EDP.ReturnValue)
        Friend ReadOnly RegexVideo_Video_All As RParams = RParams.DM("div class=""thumbnail-info-wrapper clearfix.+?[\r\n\s]*?\<span class=""title.+?[\r\n\s]*?\<a href=""([^""]+?)""[\s]+?title=""([^""]*?)""",
                                                                     0, RegexReturn.List, EDP.ReturnValue, UnicodeHexConverter)
        Friend ReadOnly RegexVideo_Video_Wrong As RParams = RParams.DM("div class=""thumbnail-info-wrapper clearfix.+?[\r\n\s]*?\<span class=""title.+?[\r\n\s]*?\<a href=""([^""]+?)""[\s]+?title=""([^""]*?)""[\w\W\s\r\n]+?(?=\<div class=""videoUploaderBlock)",
                                                                       0, RegexReturn.List, EDP.ReturnValue, UnicodeHexConverter)
        Private ReadOnly RegexVideo_Video_Wrong_Option As RParams = RParams.DM("div class=""thumbnail-info-wrapper clearfix.+?[\r\n\s]*?\<span class=""title.+?[\r\n\s]*?\<a href=""([^""]+?)""[\s]+?title=""([^""]*?)""[\w\W\s\r\n]+?", 0, RegexReturn.ListByMatch)
        Friend ReadOnly RegexVideo_Video_Wrong_Fields As RField() = {New RField(New RFieldOption(1, RegexVideo_Video_Wrong_Option)), New RField(New RFieldOption(2, RegexVideo_Video_Wrong_Option))}
        Friend ReadOnly RegexVideo_Video_VideoKey As RParams = RParams.DMS("viewkey=([\w\d]+)", 1, EDP.ReturnValue)
        Friend ReadOnly RegexVideoPageTitle As RParams = RParams.DMS("meta (property|name)=""[^:]+?:title"" content=""([^""]+)""", 2, EDP.ReturnValue)
#End Region
#Region "Declarations M3U8"
        Friend ReadOnly Regex_M3U8_FilesList As RParams = RParams.DM("RESOLUTION=\d+x(\d+).*?[\r\n]*?(.+?m3u8.*)", 0, RegexReturn.List, EDP.ReturnValue)
        Friend ReadOnly Regex_M3U8_FirstFileRegEx As RParams = RParams.DM(".+?m3u8.*", 0)
        Friend ReadOnly Regex_M3U8_FileUrl As RParams = RParams.DMS("((https://([^/]+)/.+?)([^/]+?m3u8))(.*)", 2, EDP.ReturnValue)
#End Region
#Region "Declarations GIF"
        Friend ReadOnly Regex_Gif_Array As RParams = RParams.DM("\<li id=""(gif\d+)"" class=""gifLi.gifVideoBlock""\>", 0, RegexReturn.List, EDP.ReturnValue)
        Friend ReadOnly Regex_Gif_UrlName As RParams = RParams.DMS("""name"":.*?""([^""]*)""[^\}]+?""contentUrl"":.*?""([^""]+)""", 0, RegexReturn.ListByMatch, EDP.ReturnValue)
#End Region
#Region "Declarations photo"
        Friend ReadOnly Regex_Photo_ModelHub_PhotoBlocks As RParams = RParams.DM("var PHOTOS_ARRAY_(\d+) = \{[\r\n\s]*?(urls:.*?\[[^]]*\])", 0, RegexReturn.List, EDP.ReturnValue)
        Friend ReadOnly Regex_Photo_PornHub_PhotoBlocks As RParams = RParams.DM("photoAlbumListContainer[\r\n\s\S]+?title=""([^""]+)""[\r\n\s\S]+?a href=""(/album/\d+)""", 0, RegexReturn.List)
        Friend ReadOnly Regex_Photo_PornHub_AlbumPhotoArr As RParams = RParams.DMS("\<a href=""(/photo/\d+)""", 1, RegexReturn.List, EDP.ReturnValue,
                                                                                   CType(Function(Input$) If(Input.IsEmptyString, String.Empty, $"https://www.pornhub.com{Input.Trim}"), Func(Of String, String)))
        Friend ReadOnly Regex_Photo_PornHub_SinglePhoto As RParams = RParams.DMS("(?<!thumbImage.+?)<img src=""(https://[^""]+\d+[^""]+)""", 1, EDP.ReturnValue)
        Friend ReadOnly Regex_Photo_File As RParams = RParams.DM("\d+\.[\w]{3,4}", 0, EDP.ReturnValue)
#End Region
    End Module
End Namespace