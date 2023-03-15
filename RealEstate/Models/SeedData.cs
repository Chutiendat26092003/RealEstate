using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NuGet.ProjectModel;
using RealEstate.Data;
using RealEstate.Helpper;
using RealEstate.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace RealEstate.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RealEstateContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<RealEstateContext>>()))
            {
                //Look for any students.
                if (context.Account.Any())
                {
                    return;   // DB has been seeded
                }

                //Account
                var accounts = new Account[]
                {   //Status: 0 - admin, 1 - user
                    new Account { FullName = "Addmin", Email = "admin@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123456"), Status = '0'},
                    new Account { FullName = "Michel", Email = "michel123@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123456"), Status = '1'},
                    new Account { FullName = "Recce Jame", Email = "jame123@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123456"), Status = '1'},
                    //new Account { Username = "michel", Password = BCrypt.Net.BCrypt.HashPassword("123456"), Status = '0'},
                };

                foreach (Account a in accounts)
                {
                    context.Account.Add(a);
                }
                context.SaveChanges();

                //User
                var users = new User[]
                {
                    new User { FullName = "Addmin", Address = "Hà Nội", Email="admin@gmail.com", Phone="0399999999", Balance = 1000, Image="",  AccountId  = accounts.Single( a => a.Email == "admin@gmail.com").Id },
                    new User { FullName = "Michel", Address = "Hà Nội", Email="michel123@gmail.com", Phone="0322222229", Balance = 1000, Image="",  AccountId  = accounts.Single( a => a.Email == "michel123@gmail.com").Id },
                    new User { FullName = "Recce Jame", Address = "Hà Nội", Email="jame123@gmail.com", Phone="0399995678", Balance = 1000, Image="",  AccountId  = accounts.Single( a => a.Email == "jame123@gmail.com").Id },
                };

                foreach (User u in users)
                {
                    context.User.Add(u);
                }
                context.SaveChanges();



                //Category
                var categories = new Category[]
                {
                    new Category { NameCategory = "Biệt thự", ImageURL = "collection_1.png", BannerURL = "banner_project_1.png"},
                    new Category { NameCategory = "Nhà vườn", ImageURL = "collection_2.png", BannerURL = "banner_project_2.png"},
                    new Category { NameCategory = "Nhà phố", ImageURL = "collection_3.png" , BannerURL = "banner_project_3.png"},
                    new Category { NameCategory = "Chung cư", ImageURL = "collection_4.png", BannerURL = "banner_project_4.png"},
                    new Category { NameCategory = "Căn hộ", ImageURL = "collection_5.png", BannerURL = "banner_project_5.png"},
                };

                foreach (Category u in categories)
                {
                    context.Category.Add(u);
                }
                context.SaveChanges();

                //Product
                var products = new Product[]
                {   // Directory : 0 - Cần bán, 1 - Cho thuê
                    new Product { NameProduct = "Bán biệt thự hiện đại mới xây",Price = 12500000000, AddressProduct="Xuân Thủy - Cầu Giấy - Hà Nội", Directory = '0', Area = 96, Bedroom = 3, Restrooms = 4, HomeOrientation= "Đông Bắc",IsDeleted = false,Description="+Nhà xây khung BTCT, thiết kế 5 tầng, mỗi tầng 2 phòng khép kín.\r\n\r\n+Tầng 1 làm GARA ÔTÔ, để xe cực rộng và vệ sinh. Tầng 2 là phòng khách và bếp. Tầng 3,4 mỗi tầng 2pn vệ sinh khép kín cực rộng. Tầng 5 là phòng thờ và sân phơi.\r\n\r\nCầu thang giữa.Trang thiết bị nội thất đều nhập ngoại.\r\n\r\n+Nhà 3 mặt thoáng phòng nào cũng có cửa sổ đón ánh sáng tự nhiên. Đường trước nhà ô tô tránh, thông ra đường Phạm Văn Đồng.\r\n\r\n+Nhà gần các trường đại học như Điện Lực, HVKTQS, Du Lịch. Khu dân trí cao, dân cư đông đúc, an sinh tốt.\r\n\r\n+Phù hợp với các loại hình kinh doanh và để ở. Chủ để lại toàn bộ nội thất và bao sang tên sổ đỏ.", CategoryId  = categories.Single( a => a.NameCategory == "Biệt thự").Id },
                    new Product { NameProduct = "Bán chung cư Sunshine Riverside Tây Hồ",Price = 2500000000, AddressProduct="Âu Cơ - Tứ Liên - Tây Hồ - Hà Nội", Directory = '0', Area = 96, Bedroom = 2, Restrooms = 2, HomeOrientation= "Đông Nam",IsDeleted = false,Description="Bán chung cư Sunshine Riverside Tây Hồ, căn hộ 2 phòng ngủ, 2 WC, thiết kế hiện đại hợp lý, tầng trung, hướng view đắt giá trực diện sông Hồng và cầu Nhật Tân, ko bị chắn view.\r\nDiện tích 65m2 giá 2.6 tỷ. Giá đã bao vat và full nội thất gắn tường cao cấp từ Đức, khách hàng chỉ cần sắm đồ rời.\r\nLà căn hộ 65m2 cuối cùng của dự án có thiết kế đẹp, hợp lý 2 PN, 2 WC. (Các căn 2 ngủ bé còn lại chỉ có 1 Wc).\r\n\r\nĐiểm khác biệt của chung cư cao cấp là tiện ích vượt trội: Trường học quốc tế liên cấp 1-2, bể bơi trong nhà và ngoài trời, trung tâm mua sắm, gym, spa, phòng khám quốc tế...\r\nDịch vụ cung cấp theo tiêu chuẩn khách sạn 5 sao:\r\nĐưa đón trẻ, giúp việc theo giờ, chăm sóc vật nuôi cây cảnh, giặt là, đưa đón sân bay, đặt vé máy bay....\r\nNội thất gắn tường nhập khẩu từ Đức: Koler, Hafler...\r\n\r\nChính sách khủng tháng 10/2018:\r\n- Tặng chuyến du lịch 120-250 triệu (trừ trực tiếp vào giá).\r\n- Tặng thẻ học trường mầm non quốc tế 50 triệu.\r\n- Hỗ trợ vay vốn 65% GTCH với lãi suất 0%, ân hạn nợ gốc tới 31/03/2019.\r\n- Chiết khấu 3% đối với KH thanh toán sớm 95%.\r\n- Chiết khấu 1% từ căn thứ 2 cho khách hàng và người thân mua từ căn thứ 02 trở lên.\r\n\r\nDự án hiện đã cất nóc, đang hoàn thiện, bàn giao quý 1/2019.\r\nSacombank, HD Bank bảo lãnh tiến độ dự án.", CategoryId  = categories.Single( a => a.NameCategory == "Chung cư").Id },
                    new Product { NameProduct = "Bán căn hộ cao cấp Tây Hồ",Price = 3500000000, AddressProduct="Cầu Nhật Tân - Tây Hồ - Hà Nội", Directory = '0', Area = 90, Bedroom = 3, Restrooms = 2, HomeOrientation= "Bắc",IsDeleted = false, Description="- 2 phòng ngủ: Giá từ 2,5 - 3 tỷ với diện tích 72m2 - 87m2.\r\n- 3 phòng ngủ: Giá từ 2,9 - 4 tỷ với diện tích 93m2 - 106m2.\r\n\r\n+ Chính sách bán hàng khủng:\r\n- Tặng chuyến du lịch Châu Âu 180-250 Triệu, trừ vào giá bán.\r\n- Tặng thẻ học mầm non quốc tế trị giá 50 triệu.\r\n- Tặng 2 năm phí dịch vụ.\r\n- Ngân hàng hỗ trợ 65% với 0% lãi suất tới ngày nhận nhà 31/03/2019.\r\n- Chiết khấu 3% cho khách hàng thanh toán sớm 95%.\r\n- Bàn giao quý 1/2019, hiện dự án đã cất nóc (hình ảnh thực tế bên dưới).\r\n\r\n+ Những đặc điểm nổi bật của dự án:\r\n- Nằm trong tổng thể KĐT Ciputra, là khu đô thị đẳng cấp nhất Hà Nội, không gian sống xanh mát, trong lành.\r\n- Hệ thống giáo dục đẳng cấp với các trường quốc tế: Unis, Quốc tế Hanoi Academy, trường Quốc tế Singapore, sân golf Ciputra đẳng cấp.\r\n- Vị trí ngay vòng xuyến cây xanh cầu Nhật Tân, sở hữu các hướng view vô cùng đẹp: View Sông Hồng, cầu Nhật Tân, Hồ Tây, sân golf Ciputra.\r\n- Hệ thống giao thông quận Tây Hồ được quy hoạch đồng bộ và thông thoáng, mật độ dân cư và nhà ở thấp.\r\n- Tập trung 70% người nước ngoài, quy hoạch thành phố phát triển trục Nhật Tân - Nội Bài - Tiềm năng đầu tư rất lớn.\r\n- Mỗi tầng đều có 1 vườn treo từ 60m2 - 80m2 là không gian sinh hoạt chung với nhiều cây xanh, thiết kế đón nắng đón gió đón ánh sáng.\r\n\r\nNgân hàng Sacombank bảo lãnh tiến độ dự án nên khách hàng hoàn toàn yên tâm. Ngoài ra còn hỗ trợ lãi suất 0% tới khi nhận nhà.", CategoryId  = categories.Single( a => a.NameCategory == "Căn hộ").Id },

                    new Product { NameProduct = "Cho thuê biệt thự hiện đại",Price = 48000000, AddressProduct="Phường 15 - Bình Thạnh - Hồ Chí Minh", Directory = '1', Area = 175, Bedroom = 3, Restrooms = 2, HomeOrientation= "Đông Nam",IsDeleted = false,Description="Chính sách tháng 10/2018 siêu hấp dẫn:\r\n- Tặng chuyến du lịch 250 triệu (trừ trực tiếp vào giá).\r\n- Tặng thẻ học trường mầm non quốc tế 50 triệu.\r\n- Hỗ trợ vay vốn 65% GTCH với lãi suất 0%, ân hạn nợ gốc tới 31/03/2019.\r\n- Chiết khấu 3% đối với KH thanh toán sớm 95%.\r\n- Chiết khấu 1% từ căn thứ 2 cho khách hàng và người thân mua từ căn thứ 02 trở lên.\r\n- Hiện tại dự án đã cất nóc, đang hoàn thiện bên trong.\r\n- Dự kiến bàn giao quý I/2019, ngân hàng Sacombank, HD Bank bảo lãnh tiến độ.\r\n\r\nI. Thông tin dự án:\r\n1. Chủ đầu tư: Tập đoàn Sunshine Group.\r\n2. Vị trí dự án: Phú Thượng, Tây Hồ, Hà Nội, nằm trong quần thể khu Ciputra.\r\n- View nhìn Ciputra, Sông Hồng, Hồ Tây, Cầu Nhật Tân, công viên, bể bơi, trường học, nội khu.\r\n3. Quy mô: 3 tòa: CT1 - CT3 (31 tầng), CT2 (34 tầng).\r\n- 3 tầng khối đế (TTTM, phòng tập, phòng chiếu phim, phòng khám... ).\r\n- Loại căn hộ:\r\n2 phòng ngủ: 58 - 67 - 82 m2.\r\n3 phòng ngủ: 93 - 90 - 99 m2.\r\n- Bàn giao đầy đủ đồ nội thất liền tường của các nhãn hiệu nổi tiếng (điều hòa, bếp từ, bình nóng lạnh, tủ bếp, thiết bị vệ sinh... ).\r\n\r\nII. Tiện ích.\r\n+ Dịch vụ chuyển đồ, dọn phòng,\r\n+ Bể bơi ngoài trời & bể bơi 4 mùa.\r\n+ Dãy shophouse (liền kề) kinh doanh buôn bán.\r\n+ Trung tâm thương mại.\r\n+ Chuỗi nhà hàng, cafe.\r\n+ Gym, yoga...\r\n+ Phòng khám đa khoa.\r\n+ Phòng chiếu phim.\r\n+ Trường học liên cấp.\r\n+ Dịch vụ cung cấp tiêu chuẩn 5 sao: Đưa đón trẻ, giúp việc theo giờ, chăm sóc vật nuôi cây cảnh, giặt là, đưa đón sân bay, đặt vé máy bay....", CategoryId  = categories.Single( a => a.NameCategory == "Biệt thự").Id },
                    new Product { NameProduct = "Cho thuê căn hộ, chung cư cao cấp",Price = 24000000, AddressProduct="Yên Xá - Phúc La - Hà Đông - Hà Nội", Directory = '1', Area = 175, Bedroom = 3, Restrooms = 2, HomeOrientation= "Đông Nam",IsDeleted = false,Description="Căn hộ cao cấp Eurowindow River Park chính thức được tung ra thị trường. Chính sách siêu khủng gửi tặng khách hàng:\r\n1. Tặng 01 chỉ vàng cho 30 khách hàng đầu tiên đặt cọc đủ 50 triệu kể từ ngày 01/10.\r\n2. Tặng voucher nội thất 120 triệu/căn hộ 2PN; 150 triệu/căn hộ 3PN.\r\n3. Ngân hàng Techcombank hỗ trợ 70% giá trị căn hộ trong 25 năm. Miễn lãi và ân hạn nợ gốc trong vòng 12 tháng.\r\n4. Chiết khấu 4% GTCH cho khách hàng không sử dụng vốn vay từ ngân hàng.\r\n5. Chiết khấu 10% GTCH cho khách hàng thanh toán ngay 95% khi ký HĐMB.\r\nLiên hệ: 0967 065 652 (Ms. Hiệu, quản lý kinh doanh).\r\nThông tin về căn hộ:\r\nTên dự án: Eurowindow River Park.\r\nChủ đầu tư: Tập đoàn Eurowindow Holdings.\r\nVị trí dự án: Đông Hội, Đông Trù, Đông Anh, Hà Nội.\r\nTổng diện tích dự án: 4,2ha.\r\nMật độ xây dựng: 40%.\r\nLoại hình phát triển: Biệt thự, liền kề, shophouse và căn hộ cao cấp.\r\nQuy mô dự án: Gồm 4 tòa chung cư, 65 lô biệt thự liền kề, 138 căn Shophouse, 99 căn office - tel.\r\nSố lượng căn hộ: 2058 căn hộ chung cư cao cấp.\r\nCơ cấu diện tích căn hộ: 67m2 - 73m2 - 82m2 - 96m2.\r\nĐơn thị quản lý và phát triển: Eurowindow Holdings.\r\nBàn giao: Quý IV năm 2019.\r\nGiá bán căn hộ: Từ 20 tr/m2.\r\nTiện ích dự án:\r\nDự án Eurowindow River Park với hệ thống dịch vụ tiện ích đẳng cấp cao với bể bơi bốn mùa trong ngoài trời, trung tâm thương mại, khu dịch vụ Fitness spa và chăm sóc sắc đẹp, phòng tập gym, hệ thống nhà hàng, khu vui chơi dành cho mọi lứa tuổi, hệ thống nhà trẻ mầm non và trường liên cấp quốc tế,.\r\nMặt bằng và giá bán căn hộ:\r\nGiá bán căn hộ dao động từ 20tr - 22tr/m2.\r\n- Căn hộ 67,6m2: 2PN 2WC: Giá từ 1,2 tỷ - 1,35 tỷ.\r\n- Căn hộ 71,1m2 77,6m2: 2PN 2WC: Giá từ 1,5 tỷ - 1,6 tỷ.\r\n- Căn hộ 81,2m2: 3PN 2WC: Giá từ 1,7tỷ - 1,8 tỷ.\r\n- Căn hộ 96,4m2: 3PN 2WC: Giá từ 2 tỷ - 2,2 tỷ.\r\nTiến độ thanh toán linh hoạt:\r\nĐặt cọc: 50 triệu.\r\nĐợt 1: 15% GTCH: Ký HĐMB (bao gồm 50tr đặt cọc).\r\nĐợt 2: 15% GTCH: 30/11/2018.\r\nĐợt 3: 10% GTCH: 30/01/2019.\r\nĐợt 4: 10% GTCH: 30/03/2019.\r\nĐợt 5: 10% GTCH: 30/05/2019.\r\nĐợt 6: 10% GTCH: 30/07/2019.\r\nĐợt 7: 25% GTCH: Nhận bàn giao căn hộ.\r\nĐợt 8: 5% GTCH: Bàn giao sổ hồng.", CategoryId  = categories.Single( a => a.NameCategory == "Chung cư").Id },
                };

                foreach (Product p in products)
                {
                    context.Product.Add(p);
                }
                context.SaveChanges();

                //ProductImage
                var productImages = new ProductImage[]
                {
                    new ProductImage { NameImage =  "bietthumoixay_1.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán biệt thự hiện đại mới xây").Id},
                    new ProductImage { NameImage =  "bietthumoixay_2.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán biệt thự hiện đại mới xây").Id},
                    new ProductImage { NameImage =  "bietthumoixay_3.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán biệt thự hiện đại mới xây").Id},
                    new ProductImage { NameImage =  "bietthumoixay_4.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán biệt thự hiện đại mới xây").Id},
                    new ProductImage { NameImage =  "bietthumoixay_5.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán biệt thự hiện đại mới xây").Id},
                    new ProductImage { NameImage =  "bietthumoixay_6.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán biệt thự hiện đại mới xây").Id},

                    new ProductImage { NameImage =  "chungcuSunshineRiverside_1.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán chung cư Sunshine Riverside Tây Hồ").Id},
                    new ProductImage { NameImage =  "chungcuSunshineRiverside_2.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán chung cư Sunshine Riverside Tây Hồ").Id},
                    new ProductImage { NameImage =  "chungcuSunshineRiverside_3.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán chung cư Sunshine Riverside Tây Hồ").Id},
                    new ProductImage { NameImage =  "chungcuSunshineRiverside_4.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán chung cư Sunshine Riverside Tây Hồ").Id},
                    new ProductImage { NameImage =  "chungcuSunshineRiverside_5.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán chung cư Sunshine Riverside Tây Hồ").Id},
                    new ProductImage { NameImage =  "chungcuSunshineRiverside_6.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán chung cư Sunshine Riverside Tây Hồ").Id},

                    new ProductImage { NameImage =  "can-ho-TayHo_1.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán căn hộ cao cấp Tây Hồ").Id},
                    new ProductImage { NameImage =  "can-ho-TayHo_2.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán căn hộ cao cấp Tây Hồ").Id},
                    new ProductImage { NameImage =  "can-ho-TayHo_3.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán căn hộ cao cấp Tây Hồ").Id},
                    new ProductImage { NameImage =  "can-ho-TayHo_4.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán căn hộ cao cấp Tây Hồ").Id},
                    new ProductImage { NameImage =  "can-ho-TayHo_5.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán căn hộ cao cấp Tây Hồ").Id},
                    new ProductImage { NameImage =  "can-ho-TayHo_6.jpg",  ProductId  = products.Single( a => a.NameProduct == "Bán căn hộ cao cấp Tây Hồ").Id},

                    new ProductImage { NameImage =  "thuebietthuhiendatbinhthanh_1.jpg",  ProductId  = products.Single( a => a.NameProduct == "Cho thuê biệt thự hiện đại").Id},
                    new ProductImage { NameImage =  "thuebietthuhiendatbinhthanh_2.jpg",  ProductId  = products.Single( a => a.NameProduct == "Cho thuê biệt thự hiện đại").Id},
                    new ProductImage { NameImage =  "thuebietthuhiendatbinhthanh_3.jpg",  ProductId  = products.Single( a => a.NameProduct == "Cho thuê biệt thự hiện đại").Id},
                    new ProductImage { NameImage =  "thuebietthuhiendatbinhthanh_4.jpg",  ProductId  = products.Single( a => a.NameProduct == "Cho thuê biệt thự hiện đại").Id},
                    new ProductImage { NameImage =  "thuebietthuhiendatbinhthanh_5.jpg",  ProductId  = products.Single( a => a.NameProduct == "Cho thuê biệt thự hiện đại").Id},
                    new ProductImage { NameImage =  "thuebietthuhiendatbinhthanh_6.jpg",  ProductId  = products.Single( a => a.NameProduct == "Cho thuê biệt thự hiện đại").Id},


                    new ProductImage { NameImage =  "chothuechungcu_1.jpg",  ProductId  = products.Single( a => a.NameProduct == "Cho thuê căn hộ, chung cư cao cấp").Id},
                    new ProductImage { NameImage =  "chothuechungcu_2.jpg",  ProductId  = products.Single( a => a.NameProduct == "Cho thuê căn hộ, chung cư cao cấp").Id},
                    new ProductImage { NameImage =  "chothuechungcu_3.jpg",  ProductId  = products.Single( a => a.NameProduct == "Cho thuê căn hộ, chung cư cao cấp").Id},
                    new ProductImage { NameImage =  "chothuechungcu_4.jpg",  ProductId  = products.Single( a => a.NameProduct == "Cho thuê căn hộ, chung cư cao cấp").Id},
                    new ProductImage { NameImage =  "chothuechungcu_5.jpg",  ProductId  = products.Single( a => a.NameProduct == "Cho thuê căn hộ, chung cư cao cấp").Id},
                    new ProductImage { NameImage =  "chothuechungcu_6.jpg",  ProductId  = products.Single( a => a.NameProduct == "Cho thuê căn hộ, chung cư cao cấp").Id},


                };

                foreach (ProductImage p in productImages)
                {
                    context.ProductImage.Add(p);
                }
                context.SaveChanges();

                //Transaction
                var transactions = new Transaction[]
                {   //Status: 0 - cộng, 1 - trừ
                    new Transaction { Status = '1', Amount = 70, UserId  = users.Single( a => a.FullName == "Michel").Id},
                    new Transaction { Status = '1', Amount = 70, UserId  = users.Single( a => a.FullName == "Recce Jame").Id},
                    new Transaction { Status = '1', Amount = 70, UserId  = users.Single( a => a.FullName == "Michel").Id},
                    new Transaction { Status = '1', Amount = 70, UserId  = users.Single( a => a.FullName == "Recce Jame").Id},
                    new Transaction { Status = '1', Amount = 70, UserId  = users.Single( a => a.FullName == "Recce Jame").Id},
                    new Transaction { Status = '1', Amount = 70, UserId  = users.Single( a => a.FullName == "Recce Jame").Id},
                };

                foreach (Transaction p in transactions)
                {
                    context.Transaction.Add(p);
                }
                context.SaveChanges();

                //TransactionExcept
                var transactionExcepts = new TransactionExcept[]
                {   //ServicePackage: 0 - gói 1 tuần, 1 - gói 1 tháng
                    new TransactionExcept { Time = DateTime.Parse("2022-12-29 15:20:00"), Day = 15, ServicePackage = "vang", EndTime = false, TransactionId  = 1, UserId  = users.Single( a => a.FullName == "Michel").Id, ProductId  = products.Single( a => a.NameProduct == "Bán biệt thự hiện đại mới xây").Id},
                    new TransactionExcept { Time = DateTime.Parse("2022-12-29 16:20:00"), Day = 15, ServicePackage = "vang", EndTime = false, TransactionId  = 2, UserId  = users.Single( a => a.FullName == "Recce Jame").Id, ProductId  = products.Single( a => a.NameProduct == "Bán chung cư Sunshine Riverside Tây Hồ").Id},
                    new TransactionExcept { Time = DateTime.Parse("2022-12-29 17:20:00"), Day = 15, ServicePackage = "vang", EndTime = false, TransactionId  = 3, UserId  = users.Single( a => a.FullName == "Michel").Id, ProductId  = products.Single( a => a.NameProduct == "Bán căn hộ cao cấp Tây Hồ").Id},
                    new TransactionExcept { Time = DateTime.Parse("2022-12-29 18:20:00"), Day = 15, ServicePackage = "vang", EndTime = false , TransactionId  = 4, UserId  = users.Single( a => a.FullName == "Recce Jame").Id, ProductId  = products.Single( a => a.NameProduct == "Cho thuê biệt thự hiện đại").Id},
                    new TransactionExcept { Time = DateTime.Parse("2022-12-29 22:20:00"), Day = 15, ServicePackage = "vang", EndTime = false, TransactionId  = 5, UserId  = users.Single( a => a.FullName == "Recce Jame").Id, ProductId  = products.Single( a => a.NameProduct == "Cho thuê căn hộ, chung cư cao cấp").Id},
                };

                foreach (TransactionExcept p in transactionExcepts)
                {
                    context.TransactionExcept.Add(p);
                }
                context.SaveChanges();

            }
        }
    }
}
