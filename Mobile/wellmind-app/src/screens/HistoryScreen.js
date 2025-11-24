import { useEffect, useState } from 'react';
import { Alert, Button, FlatList, Text, View } from 'react-native';
import { deleteEntry, getEntries } from '../api/wellbeingService';

export default function HistoryScreen() {
  const [items, setItems] = useState([]);
  const load = async () => {
    try { const res = await getEntries(); setItems(res); } catch (e) {}
  };
  useEffect(()=>{ load(); },[]);
  const onDelete = (id) => {
    Alert.alert('Confirmar', 'Excluir item?', [
      {text:'Cancelar'},
      {text:'OK', onPress: async ()=>{ await deleteEntry(id); load(); }}
    ]);
  };
  return (
    <View style={{flex:1}}>
      <FlatList data={items} keyExtractor={(i)=>String(i.id)} renderItem={({item})=>(
        <View style={{padding:12,borderBottomWidth:1}}>
          <Text>{item.timestamp} — {item.emotion} ({item.confidence})</Text>
          <View style={{flexDirection:'row',marginTop:8}}>
            <Button title="Editar" onPress={()=>Alert.alert('Editar','Implemente edição')} />
            <Button title="Excluir" onPress={()=>onDelete(item.id)} />
          </View>
        </View>
      )} />
    </View>
  );
}
