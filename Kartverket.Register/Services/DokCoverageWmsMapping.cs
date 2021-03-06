﻿using System.Collections.Generic;

namespace Kartverket.Register.Services
{
    public class DokCoverageWmsMapping
    {
        public static readonly Dictionary<string, string> DatasetUuidToWmsLayerMapping = new Dictionary<string, string>()
        {
            {"edddd150-1cec-4edb-90fd-06806c1f585d", "avinor_lufthavn_restriksjonsplaner"},
            {"1489f7f8-40c8-4dc4-83b6-bcf277b56506", "avinor_stoysoner"},
            {"b3c319bd-910d-4663-8ce8-23a246afe879", "dm_bergrettigheter"},
            {"ceef6c79-27ea-4e3e-895d-33d2a64763bf", "dsb_eksplosivanlegg"},
            {"12fa3360-ce91-4f02-82c8-22ff85cf0c67", "dsb_farligstoff"},
            {"63190f80-8692-492a-8e7a-b2cb0a59d27a", "dsb_storulykkeanlegg"},
            {"17c16f2d-7b44-49cf-b57c-8e8b3b6ef011", "fdir_akvakulturlokalitet"},
            {"33c6b359-1379-4b6a-9f42-4bd13c67616e", "fdir_fiskeplass_aktive_redsk"},
            {"09a40026-00e2-4fd8-b390-afd8d6f88c63", "fdir_fiskeplass_passive_redsk"},
            {"c3d6bba0-e4b0-44e6-93a4-5b379946acb7", "fdir_gyteomrade"},
            {"b12d041d-03d4-4562-b333-bc9d7c9bd123", "fdir_laksefjorder"},
            {"4d0bd656-640b-4592-be35-e7435aa313dc", "fdir_tare_hostefelt"},
            {"166382b4-82d6-4ea9-a68e-6fd0c87bf788", "fkb_ar5"},
            {"87b31015-a3de-4540-9b8b-cb1bf4e1cb3a", "fkb_arealbruk"},
            {"3165138f-1461-44fe-8b10-eac44e08a10a", "fkb_bane"},
            {"ede5ffb2-ee2a-44a3-852d-369a14d97f2e", "fkb_bygnanlegg"},
            {"8b4304ea-4fb0-479c-a24d-fa225e2c6e97", "fkb_bygning"},
            {"b49478fd-038e-4c2c-ae28-dda1958a8048", "fkb_hoydekurve"},
            {"6e05aefb-f90e-4c7d-9fb9-299574d0bbf6", "fkb_ledning"},
            {"128248ad-f4f0-4543-901e-5e0c3bbaecfc", "fkb_ledningva"},
            {"23dfcc33-fb04-4898-aa88-68b49c4bfea7", "fkb_lufthavn"},
            {"c9e53371-c296-4631-a08d-2e7248a81757", "fkb_n20_kartdata"},
            {"6bb353c3-2b21-42fe-b296-31e60f64f95d", "fkb_n5_kartdata"},
            {"bd23cc07-45d3-4346-a8cd-e8c9f506656e ", "fkb_n5_raster"},
            {"aa3c01f3-0678-470d-b03b-33085a7bae28", "fkb_naturinfo"},
            {"8944603c-9414-43a7-9421-9a1de9850a96", "fkb_pbltiltak"},
            {"7e39afd2-5af6-435f-a859-5a86d136945b", "fkb_servitutt"},
            {"9bc229df-3f97-476e-9fb2-093793356f9a", "fkb_tekst1000"},
            {"42104679-7712-45e5-b09d-8a99967ae060", "fkb_tekst5000"},
            {"cc3a2d98-52ac-4699-9947-ed0625903de4", "fkb_traktorvegsti"},
            {"595e47d9-d201-479c-a77d-cbc1f573a76b", "fkb_vann"},
            {"4920b452-75cc-45f2-964c-3378204c3517", "fkb_veg"},
            {"d80faca7-2a0d-47de-b58b-5007a2afdc74", "fm_barmarksloyper"},
            {"1ee8b824-809a-41f9-8877-01bb717cbd8f", "fm_vernskog"},
            {"21d93ab2-868f-4ef9-986b-bf362746f4bd", "forsvaret_stoysoner"},
            {"c3da3591-cded-4584-a4b1-bc61b7d1f4f2", "jbv_banenettverk"},
            {"47234f63-a9d1-43c7-b91c-db90c92d5008", "jbv_stoysoner_jernbane"},
            {"31edb985-138e-46a7-a910-a0c1cd9baf4c", "korallrev"},
            {"041f1e6e-bdbc-4091-b48f-8a5990f3cc5b", "kv_adminomr_kommune"},
            {"e106adf4-c9d8-4fce-a9b5-7886a4126d23", "kv_adm_norges_maritime_grenser"},
            {"dddbb667-1303-4ac5-8640-7ec04c0e3918", "kv_dtm_10"},
            {"e25d0104-0858-4d06-bba8-d154514c11d2", "kv_dtm_50"},
            {"2751aacf-5472-4850-a208-3532a51c529a", "kv_dybdedata"},
            {"871960a1-0f01-4c47-8f79-5d338b65197e", "kv_dybdekurver"},
            {"45e9ba77-388c-40eb-87df-e34e9d9ab300", "kv_lovomrade_markalov"},
            {"e77e6fdc-591d-4b1b-91b2-bd9d13fb33b7", "kv_matrikkel"},
            {"f7df7a18-b30f-4745-bd64-d0863812350c", "kv_matrikkel_adresse"},
            {"24d7e9d1-87f6-45a0-b38e-3447f8d7f9a1", "kv_matrikkel_bygningspkt"},
            {"13336c7c-e150-4497-915a-281f3bd34a2e", "kv_matrikkel_eiendom"},
            {"aee42bb6-d0e9-4d70-86fe-6ea76c381055", "kv_n1000_kartdata"},
            {"b187c449-04ac-42fe-9eab-83d81bf338bc", "kv_n20_bygning"},
            {"442cae64-b447-478d-b384-545bc1d9ab48", "kv_n250_kartdata"},
            {"ea192681-d039-42ec-b1bc-f3ce04c189ac", "kv_n50_kartdata"},
            {"c777d53d-8916-4d9d-bae4-6d5140e0c569", "kv_n5000_kartdata"},
            {"d5b180dd-0cef-4c4a-9174-ba5af69c3551", "kv_ortofoto"},
            {"3b1cbed1-5537-4f61-90df-3b396c656813", "kv_planlegg_barnetråkk"},
            {"39e022aa-c352-4351-b172-b980b03c578c", "kv_sjo_terrengmodeller"},
            {"09d3a23e-7652-4f15-8248-968afae350f5", "kv_sjokart_raster"},
            {"19d0a0df-3320-4984-9340-f11f143ff14a", "kv_sjokart_vektor"},
            {"30caed2f-454e-44be-b5cc-26bb5c0110ca", "kv_stedsnavn_ssr"},
            {"638b5ee7-6ab0-4a27-a71a-716bb3a4541d", "kv_tilgjengelighet_friluft"},
            {"9c075b5d-1fb5-414e-aaf5-c6390db896d1", "kv_tilgjengelighet_tettsted"},
            {"d1422d17-6d95-4ef1-96ab-8af31744dd63", "kv_tur_og_friluftsruter"},
            {"96104f20-15f6-460e-a907-501a65e2f9ce", "kv_vbase"},
            {"d6a20e09-fa68-4d57-ab43-c22e755ff60a", "kyv_ankringsomrader"},
            {"86368a3e-a808-49d2-937e-6b0d0bedeae3", "kyv_fartsforskrifter"},
            {"8ff1538a-a93c-4391-8d6f-3555fc37819c", "kyv_hovedbiled"},
            {"1e8d7811-87ea-429a-8d97-4ea6bbf6e010", "kyv_hovedled_biled_areal"},
            {"94b8c392-e2c8-426a-8dbe-ae828049a1df", "kyv_navi_installasjon"},
            {"14771301-a73b-4c95-8270-5822f9b1510c", "kyv_opplagsomrader"},
            {"91a9a937-3c03-44e2-9dd8-c347031e52a9", "kyv_riggomrader"},
            {"c675e35f-c1c5-409e-8dbb-9a1342f4aa09", "ldir_avtaleomrader"},
            {"df2db95d-adbc-4807-bb46-00b729caed7c", "ldir_beitehage"},
            {"1c64c5ff-0069-4f8e-9a2b-948c7ce3d527", "ldir_ekspropriasjonsomr"},
            {"f9c1e228-892f-4f1a-9e4e-b6d6149f373c", "ldir_flyttlei"},
            {"6383f5a8-3a4d-48fc-8c67-f1eeec24fd8b", "ldir_hostbeite"},
            {"85a4c5e3-25ab-427c-b664-bbac2d0c9e79", "ldir_hostvinterbeite"},
            {"49efb2b2-93e3-4175-b10b-65b509d73c2a", "ldir_konsesjonsomrader"},
            {"5cb86063-3f66-4d7a-9799-0551e2e21a46", "ldir_konvensjonsomrade"},
            {"adee0d1d-502a-4829-a446-dc227829f019", "ldir_oppsamlingsomrade"},           
            {"6bfec384-92cf-44d3-863b-0187afa06658", "ldir_reinbeitedistrikt"},
            {"7f88f401-4e0e-4be6-9e12-265c7b23505d", "ldir_reindrift_avtaleomrade"},
            {"bc5e4c60-9292-4a30-8417-28a193ec4569", "ldir_reinbeiteomrade"},
            {"a02e84ec-322c-47a7-a626-ca02d57d1f7e", "ldir_reindrift_oppsamlingsomr"},
            {"8dfa67c5-3099-4353-9ce0-72f9ebd44a2c", "ldir_reindriftsanlegg"},
            {"05a9cb73-639e-48ab-8533-bd80521a84bf", "ldir_restriksjonsomrader"},
            {"00828ff6-b1e4-4916-9d65-50199e293c1e", "ldir_siidagrense"},
            {"d5d1e2d4-7dc0-47ce-8776-ff64b07d788e", "ldir_sommerbeite"},
            {"a9caf1ff-5c30-4acc-ace9-6bcb486fbb47", "ldir_tamreinlag"},
            {"95b0d396-a6fe-462b-8753-120efd0b60f3", "ldir_trekklei"},
            {"fa02a652-cd6d-4828-9fb5-7bd4515aa6d0", "ldir_varbeite"},
            {"63f655ef-f625-43cf-a512-bb8164bf53a4", "ldir_vinterbeite"},
            {"a8456aed-441a-40c4-831f-46bcbe4e6ff1", "md_arter_nasjon_forv"},
            {"e48e71ac-16fc-4e47-9e7f-c0a4a4bbfad0", "md_forurensetgrunn"},
            {"91e31bb7-356f-4478-bcba-d5c2de6e91bc", "md_frilkartlverds"},
            {"e08ad2cb-3262-4a78-bc58-2f3b5c5ccfd2", "md_frilstatligsikra"},
            {"77512fbd-cfc5-497a-8c41-ebaf5f736ded", "md_naturt_landskap"},
            {"2c0072de-f702-401e-bfb3-5ad3d08d4c2d", "md_naturt_utv"},
            {"5857ec0a-8d2c-4cd8-baa2-0dc54ae213b4", "md_naturvernomr"},
            {"0e7eb17f-35ef-46d3-a465-3112d8bb2b5e", "md_naturvernomr_foresl"},
            {"07e6b8af-84a7-43cb-9d91-887885a7342f", "md_snoskuterloyper"},
            {"b203e422-5270-4efc-93a5-2073725c43ef", "md_vannforekomster"},
            {"d7783063-3f0f-4a61-a70d-5e11b6ea662a", "md_vannskuter_forbudsomr"},
            {"a6368bed-4896-41d3-92aa-cc2b4261adc3", "md_verdif_kulturlandsk"},
            {"d776ff93-104d-4aa5-a8d9-276df01eb51c", "md_viktige_naturt"},
            {"fc59e9a4-59df-4eb3-978a-1c173b84bf4e", "md_villreinomr"},
            {"52a55a3c-bcd2-44d7-ac21-2f4550161937", "ngu_geologisk_naturarv"},
            {"82cd33ef-52dd-4c83-b2d6-e55a0941b33b", "ngu_grunnvannsborehull"},
            {"a26e57bc-15bd-46db-8504-6c6ed1e7c501", "ngu_grusogpukk"},
            {"3de4ddf6-d6b8-4398-8222-f5c47791a757", "ngu_losmasser"},
            {"cf8ccec7-9505-4d84-94a9-eac9c69971d3", "ngu_maringrense"},
            {"641a81c2-9354-473b-b8f0-2b1e2ab3930a", "ngu_marinleire_mulighet"},
            {"378d5f88-de1f-4d1e-b99a-7b0f0dd52951", "ngu_mineralressurser"},
            {"bf45a463-434d-4b4d-84dc-9325780ab5fb", "ngu_nadag_grunnundersokelser"},
            {"dc0605f3-2301-4abe-a91f-6da42464c281", "ngu_radonaktsomhet"},
            {"d5ab0e04-421b-4a04-ba46-8921b57951b2", "nibio_ar50"},
            {"41f6b000-c394-41c5-8ebb-07a0a3ec914f", "nibio_ar50_arealtyper"},
            {"8252baea-5bad-428b-8f18-fe236fa4ced6", "nibio_dyrkbarjord"},
            {"112d6261-f649-4668-ab3a-8d6ed3fc63d9", "nibio_jordkvalitet"},
            {"30e1883e-70e9-4510-9e97-00edbdcddc02", "nve_aktsomhet_jordflomskred"},
            {"e9542260-6315-4770-b085-86e95179f735", "nve_aktsomhet_snoskred"},
            {"b68212a2-ecb4-4733-8964-2882dc2ea363", "nve_aktsomhet_snosteinsprang"},
            {"02c6d51c-4e8c-4948-a620-dc046c8cb747", "nve_aktsomhet_steinsprang"},
            {"8c906c83-5192-4c2b-86ca-0d85759d37b8", "nve_dam"},
            {"60c5024f-bf93-4d7a-888a-5fe001427195", "nve_flom_aktsomhet"},
            {"e95008fc-0945-4d66-8bc9-e50ab3f50401", "nve_flomsoner"},
            {"9f71a24b-9997-409f-8e42-ce6f0c62e073", "nve_kraftlinjer"},
            {"a29b905c-6aaa-4283-ae2c-d167624c08a8", "nve_kvikkleire"},
            {"5d13cd67-dc03-458d-8715-548bafbeb2c1", "nve_magasin"},
            {"890fe9ea-5111-459b-aebd-450480c7713a", "nve_omradekonsesjoner"},
            {"bc27d7aa-afaf-43c2-802f-6a47f792b2aa", "nve_sikringstiltak"},
            {"101c2686-a57a-4a4a-9b77-654accc3a967", "nve_sjokabler"},
            {"17149f79-1289-4e3c-b964-94113eeb14c8", "nve_skred_fjellskred_store"},
            {"b2d5aaf8-79ac-40f3-9cd6-fdc30bc42ea1", "nve_skredsoner"},
            {"73c9cc00-c541-43b3-a825-9ae3ba13f4ad", "nve_vanninntak"},
            {"f587a15a-c72a-4b21-aae9-4132df1bdd27", "nve_vannkraft"},
            {"1dc3a086-cdf8-4791-86d8-159123437315", "nve_vannvei"},
            {"df5c70a1-e9cf-4b2c-bf9a-7dce453830cd", "nve_verneplan_vassdrag"},
            {"ac249604-cd82-490c-83cc-9cd24fe18088", "nve_vindparkomrader"},
            {"73f863ba-628f-48af-b7fa-30d3ab331b8d", "ra_brannsmitteomr"},
            {"17150d2c-b50d-4792-80f4-0cb2ec5eaa79", "ra_enkeltminner"},
            {"a4bfd879-120f-490e-9907-68ba870664b1", "ra_frededebygg"},
            {"17adbcac-bbb2-4efc-ab51-756573c8f178", "ra_kulturmiljo"},
            {"c6896f24-71f9-4203-9b6f-faf3bfe1f5ed", "ra_lokaliteter"},
            {"93f06149-037c-48cf-b294-d166f65b6838", "ra_sefrak_bygninger"},
            {"0a3251bb-2a50-45d3-8674-58bade2fe673", "ra_sikringssoner"},
            {"420a0af3-135c-49de-bd14-e4dec9c858db", "ra_tettetrehusm"},
            {"a965a979-c12a-4b26-90a0-f09de47dbecd", "ssb_arealbruk"},
            {"d1bc3c36-959f-4ee4-bcc7-01caa430bce4", "ssb_befolkning_rutenett1000m"},
            {"fa79e0b3-3b66-4fd1-8d0c-07fd59a7e168", "ssb_befolkning_rutenett250m"},
            {"4a0f1b2d-7b62-492b-966a-383f8d7c2b12", "sk_statligarealplan"},
            {"38ec2dd2-818d-4021-a7be-061c60e880c6", "svv_stoysoner"},
            {"af2c4a0a-1978-4e62-b08d-ed1f36bd5023", "svv_trafikkmengde"},
            {"2c47f033-b877-4885-a0ea-50333afd8fab", "svv_trafikkulykker"}
        };
    }
}