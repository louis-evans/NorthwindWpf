﻿using AutoMapper;
using Northwind.Data;
using NorthwindWpf.Core.Utils;
using System.Linq;
using WpfApp1.Models;
using WpfApp1.ViewModels;

namespace WpfApp1.Configuration
{
    public static class AutomapperConfiguration
    {
        public static void Initialise()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<OrderModelProfile>();
                cfg.AddProfile<ViewModelProfile>();
            });
        }

        protected class ViewModelProfile : Profile
        {
            public ViewModelProfile()
            {
                CreateMap<Order, OrderViewModel>()
                    .ForMember(dest => dest.OrderID, opt => opt.MapFrom(src => src.OrderID))
                    .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                    .ForMember(dest => dest.Shipper, opt => opt.MapFrom(src => src.Shipper))
                    .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                    .ForMember(dest => dest.RequiredDate, opt => opt.MapFrom(src => src.RequiredDate))
                    .ForMember(dest => dest.ShipName, opt => opt.MapFrom(src => src.ShipName))
                    .ForMember(dest => dest.ShipAddress, opt => opt.MapFrom(src => src.ShipAddress))
                    .ForMember(dest => dest.ShipCity, opt => opt.MapFrom(src => src.ShipCity))
                    .ForMember(dest => dest.ShipRegion, opt => opt.MapFrom(src => src.ShipRegion))
                    .ForMember(dest => dest.ShipPostCode, opt => opt.MapFrom(src => src.ShipPostalCode))
                    .ForMember(dest => dest.ShipCountry, opt => opt.MapFrom(src => src.ShipCountry));

                CreateMap<Order_Detail, OrderViewModel.LineItem>()
                    .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                    .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                    .ForMember(dest => dest.Qty, opt => opt.MapFrom(src => src.Quantity))
                    .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount * 100));
            }
        }

        protected class OrderModelProfile : Profile
        {
            public OrderModelProfile()
            {
                CreateMap<Order, OrderLineModel>()
                    .ForMember(dest => dest.OrderID, opt => opt.MapFrom(src => src.OrderID))
                    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Customer.CompanyName))
                    .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                    .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.Order_Details.Count()))
                    .ForMember(dest => dest.OrderTotal, opt => opt.MapFrom(src => src.Order_Details.Sum(x => OrderUtils.CalculateLineTotal(x.UnitPrice, x.Quantity, x.Discount))));
            }
        }
    }
}