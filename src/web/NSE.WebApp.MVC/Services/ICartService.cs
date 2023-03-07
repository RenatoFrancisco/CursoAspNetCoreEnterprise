﻿namespace NSE.WebApp.MVC.Services;

public interface ICartService
{
    Task<CartViewModel> GetAsync();
    Task<ResponseResult> AddItemCartAsync(ItemProductViewModel product);
    Task<ResponseResult> UpdateItemCartAsync(Guid productId, ItemProductViewModel product);
    Task<ResponseResult> RemoveItemCartAsync(Guid productId);
}
