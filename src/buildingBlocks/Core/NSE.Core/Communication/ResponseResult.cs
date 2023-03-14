﻿namespace NSE.Core.Communication;

public class ResponseResult
{
    public string Title { get; set; }
    public int Status { get; set; }
    public ResponseErrorMessages Errors { get; set; } = new ResponseErrorMessages();
}
