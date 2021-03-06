﻿using Kartverket.Register.Models;
using Kartverket.Register.Models.ViewModels;

namespace Kartverket.Register.Services
{
    public interface IInspireDatasetService
    {
        InspireDataset GetInspireDatasetByName(string registerSeoName, string itemSeoName);
        InspireDataset NewInspireDataset(InspireDatasetViewModel inspireDatasetViewModel, string parentregister, string registername);
        InspireDatasetViewModel NewInspireDatasetViewModel(string parentRegister, string register);
        InspireDataset UpdateInspireDataset(InspireDatasetViewModel viewModel);
        InspireDataset UpdateInspireDatasetFromKartkatalogen(InspireDataset inspireDataset);
        void DeleteInspireDataset(InspireDataset inspireDataset);
    }
}
