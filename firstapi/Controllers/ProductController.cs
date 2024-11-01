using firstapi.Data;
using firstapi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace firstapi.Controllers
{
   
    [Route("api/[Controller]")]

    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly MydbContext conn;
        private readonly IConfiguration configuration;
        public ProductController(IConfiguration configuration)
        {
            conn = new MydbContext();
            this.configuration = configuration;
        }
        //for get data
        
        [HttpGet("GetProducts")]
        public IActionResult Get()
        {

            var products = conn.Products.ToList();
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
//for insert data

        [HttpPost("Add")]
        public IActionResult AddProduct(int id, [FromBody] Product product)
        {
            if(product == null)
            {
                return BadRequest();
            }
            var products = new Product
            {
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
            };
            conn.Products.Add(products);
            conn.SaveChanges();
            return Ok(products);
        }

//Update Api

        [HttpPut("{id}")]
        public IActionResult updateproduct(int id, [FromBody] Product prd)
        {
            if (prd == null || id != prd.pId)
            {
                return BadRequest();
            }
            var existingproducts = conn.Products.FirstOrDefault(m => m.pId == id);
            if (existingproducts == null)
            {
                return NotFound();
            };
            existingproducts.ProductName = prd.ProductName;
            existingproducts.ProductPrice = prd.ProductPrice;

            conn.Update(existingproducts);
            conn.SaveChanges();
            return Ok(existingproducts);
        }


//delete api
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromBody] Product pd)
        {
            if(pd == null || id != pd.pId)
            {
                return BadRequest();
            }
            var existproduct = conn.Products.FirstOrDefault(z=> z.pId == id);
            if(existproduct == null)

            {
                return NotFound();
            }
            conn.Remove(existproduct);
            conn.SaveChanges();
            return Ok(existproduct);
        }




        //LoginApi 
        [HttpPost("Login")]
        public IActionResult LoginApi([FromBody] user u)
        
        {
          var users =  conn.Users.Where(i => i.UserName == u.UserName && i.Password == u.Password).FirstOrDefault();
            if(users == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("UserName",users.UserName),
                new Claim("Password",users.Password),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                signingCredentials: signin
);
            string tokenvalue = new JwtSecurityTokenHandler().WriteToken(token);
          


            return Ok(new { token = tokenvalue ,User = users});
        }



        [HttpPost("User")]
        public IActionResult adding([FromBody] user uu)
        {
            if(uu == null)
            {
                return BadRequest();
            }
            var User = new user
            {
                UserName = uu.UserName,
                Password = uu.Password,

            };
            conn.Users.Add(User);
            conn.SaveChanges();
            return Ok(User);
        }

        [Authorize]
        [HttpGet("GetUsersby")]
        public IActionResult user(user us)
        {
            var data = conn.Users.ToList();
            if(data == null)
            {
                return BadRequest();
            }
            return Ok(data);
        }
        

     
        

    }
}



