package com.wellmind.config;

import com.fasterxml.jackson.annotation.JsonTypeInfo;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.jsontype.impl.LaissezFaireSubTypeValidator;
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule;
import org.springframework.cache.CacheManager;
import org.springframework.cache.annotation.CachingConfigurer;
import org.springframework.cache.annotation.EnableCaching;
import org.springframework.cache.interceptor.CacheErrorHandler;
import org.springframework.cache.interceptor.SimpleCacheErrorHandler;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.redis.cache.RedisCacheConfiguration;
import org.springframework.data.redis.cache.RedisCacheManager;
import org.springframework.data.redis.connection.RedisConnectionFactory;
import org.springframework.data.redis.serializer.GenericJackson2JsonRedisSerializer;
import org.springframework.data.redis.serializer.RedisSerializationContext;
import org.springframework.data.redis.serializer.StringRedisSerializer;

import java.time.Duration;
import java.util.HashMap;
import java.util.Map;

@Configuration
@EnableCaching
public class CacheConfig implements CachingConfigurer {

    /**
     * Configura o gerenciador de cache Redis com TTL personalizados
     */
    @Bean
    public CacheManager cacheManager(RedisConnectionFactory connectionFactory) {

        // Configuração padrão para todos os caches
        RedisCacheConfiguration defaultConfig = RedisCacheConfiguration.defaultCacheConfig()
                .entryTtl(Duration.ofMinutes(30))  // TTL padrão: 30 minutos
                .disableCachingNullValues()         // Não cacheia valores nulos
                .serializeKeysWith(
                        RedisSerializationContext.SerializationPair.fromSerializer(
                                new StringRedisSerializer()
                        )
                )
                .serializeValuesWith(
                        RedisSerializationContext.SerializationPair.fromSerializer(
                                new GenericJackson2JsonRedisSerializer(objectMapper())
                        )
                );

        // Configurações específicas por cache
        Map<String, RedisCacheConfiguration> cacheConfigurations = new HashMap<>();

        // Cache de usuários: 1 hora
        cacheConfigurations.put("usuarios",
                defaultConfig.entryTtl(Duration.ofHours(1)));

        // Cache de empresas: 2 horas (dados mudam menos)
        cacheConfigurations.put("empresas",
                defaultConfig.entryTtl(Duration.ofHours(2)));

        // Cache de registros de bem-estar: 15 minutos (dados frequentes)
        cacheConfigurations.put("registros-bemestar",
                defaultConfig.entryTtl(Duration.ofMinutes(15)));

        // Cache de recomendações: 1 hora
        cacheConfigurations.put("recomendacoes",
                defaultConfig.entryTtl(Duration.ofHours(1)));

        // Cache de categorias: 4 horas (raramente mudam)
        cacheConfigurations.put("categorias",
                defaultConfig.entryTtl(Duration.ofHours(4)));

        // Cache de profissionais de saúde: 2 horas
        cacheConfigurations.put("profissionais",
                defaultConfig.entryTtl(Duration.ofHours(2)));

        return RedisCacheManager.builder(connectionFactory)
                .cacheDefaults(defaultConfig)
                .withInitialCacheConfigurations(cacheConfigurations)
                .transactionAware() // Sincroniza com transações do banco
                .build();
    }

    /**
     * ObjectMapper customizado para serialização JSON
     * Suporta LocalDate, LocalDateTime, etc.
     */
    @Bean
    public ObjectMapper objectMapper() {
        ObjectMapper mapper = new ObjectMapper();

        // Registra módulo para tipos Java 8+ (LocalDate, LocalDateTime)
        mapper.registerModule(new JavaTimeModule());

        // Configura type info para deserialização polimórfica
        mapper.activateDefaultTyping(
                LaissezFaireSubTypeValidator.instance,
                ObjectMapper.DefaultTyping.NON_FINAL,
                JsonTypeInfo.As.PROPERTY
        );

        return mapper;
    }

    /**
     * Tratamento de erros de cache
     * Quando o Redis falha, a aplicação continua funcionando
     */
    @Override
    public CacheErrorHandler errorHandler() {
        return new SimpleCacheErrorHandler() {
            @Override
            public void handleCacheGetError(RuntimeException exception,
                                            org.springframework.cache.Cache cache,
                                            Object key) {
                // Log do erro mas não interrompe a aplicação
                System.err.println("Cache GET error: " + exception.getMessage());
                super.handleCacheGetError(exception, cache, key);
            }

            @Override
            public void handleCachePutError(RuntimeException exception,
                                            org.springframework.cache.Cache cache,
                                            Object key,
                                            Object value) {
                // Log do erro mas não interrompe a aplicação
                System.err.println("Cache PUT error: " + exception.getMessage());
                super.handleCachePutError(exception, cache, key, value);
            }
        };
    }
}
