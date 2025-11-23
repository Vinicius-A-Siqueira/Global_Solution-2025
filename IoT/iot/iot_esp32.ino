#include <WiFi.h>
#include <HTTPClient.h>

const char* ssid = "SEU_WIFI";
const char* password = "SENHA";

void setup() {
  Serial.begin(115200);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.println("Conectando...");
  }

  Serial.println("Conectado!");
}

void loop() {
  float temperatura = random(25, 34);
  float umidade = random(40, 80);
  int luz = random(50, 400);
  int ruido = random(40, 80);

  HTTPClient http;
  http.begin("http://SEU_IP/api/iot");

  http.addHeader("Content-Type", "application/json");

  String json = "{\"temperatura\":" + String(temperatura) +
                ", \"umidade\":" + String(umidade) +
                ", \"luminosidade\":" + String(luz) +
                ", \"ruido\":" + String(ruido) + "}";

  http.POST(json);

  Serial.println("Enviado: " + json);

  http.end();
  delay(5000);
}
