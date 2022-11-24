PUT /vehicles
{
  "mappings": {
    "properties": {
      "id": { "type": "keyword" },
      "description": { "type": "text" },
      "make": { "type": "keyword" },
      "model": { "type": "keyword" },
      "price": { "type": "numeric" }
    }
  }
}

POST _bulk
{ "index" : { "_index" : "vehicles", "_id" : "24a14cff-dbb8-4a61-9ec3-6b58f598da53" } }
{ "id" : "24a14cff-dbb8-4a61-9ec3-6b58f598da53", "make": "audi", "model": "A1 Sportback", "description": "The perfect urban accomplice; compact and taut, without compromising on style or detail. A dynamic design and refined interior offer new levels of spaciousness and comfort, making the Audi A1 Sportback perfect for longer adventures too.", "price": "30000.99"}
{ "index" : { "_index" : "vehicles", "_id" : "eba2f1f2-0299-4414-b26c-cdef6b80d487" } }
{ "id" : "eba2f1f2-0299-4414-b26c-cdef6b80d487", "make": "audi", "model": "A4", "description": "Polished and progressive, our powerful A4 Saloon offers the ultimate combination of athletic style and practicality for those needing a sports car with versatility. With a sharp, classic look and premium interior full of innovative technologies to aid your driving experience, the A4 Saloon offers the whole package.", "price": "50000.99"}
{ "index" : { "_index" : "vehicles", "_id" : "e2c55826-8759-4aac-8d81-fee45ef85fc4" } }
{ "id" : "e2c55826-8759-4aac-8d81-fee45ef85fc4", "make": "PEUGEOT", "model": "208", "description": "PEUGEOT 208 shows off its youthful side with its distinctive sporty shape with the interior revealing the original PEUGEOT i-Cockpit® 3D. Discover PEUGEOT's Power of Choice  commitment, this city car gives you the freedom to choose a petrol, diesel or electric engine.", "price": "25999.99"}
{ "index" : { "_index" : "vehicles", "_id" : "b208ec98-7c8e-4ee4-a40e-96ec439c7559" } }
{ "id" : "b208ec98-7c8e-4ee4-a40e-96ec439c7559", "make": "Volvo", "model": "EX90", "description": "We’re human, we make mistakes. So if life happens to sometimes distract you, we are here to help keep you safe. With the new Volvo EX90 we enter a new era for safety.", "price": "45000.99"}