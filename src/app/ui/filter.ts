
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'Filter',
  pure: false
})
export class FilterPipe implements PipeTransform {
  // transform(items: any[], field : string, value : string): any[] {  
  //   if (!items) return [];
  //   if (!value || value.length == 0) return items;
  //   return items.filter(it => it[field] == value);
  //   // return items.filter(it => it[field].toLowerCase().indexOf(value.toLowerCase()) !=-1);
  // }
  transform(items: any, filter: any, defaultFilter: boolean): any {
    if (!filter) {
      return items;
    }
    if (!Array.isArray(items)) {
      return items;
    }

    if (filter && Array.isArray(items)) {
      let filterKeys = Object.keys(filter);

      if (defaultFilter) {
        return items.filter(item =>
          filterKeys.reduce((x, keyName) =>
            (x && new RegExp(filter[keyName], 'gi').test(item[keyName])) || filter[keyName] == "", true));
      } else {
        return items.filter(item => {
          return filterKeys.some((keyName) => {
            return new RegExp(filter[keyName], 'gi').test(item[keyName]) || filter[keyName] == "";
          });
        });
      }
    }
  }
}