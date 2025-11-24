import { Button, Text, View } from 'react-native';

export default function HomeScreen({ navigation }) {
  return (
    <View style={{flex:1,justifyContent:'center',alignItems:'center'}}>
      <Text>WellMind — Bem Vindo</Text>
      <Button title="Fazer Check‑in" onPress={() => navigation.navigate('Checkin')} />
      <Button title="Ver Ambiente" onPress={() => navigation.navigate('Environment')} />
    </View>
  );
}
