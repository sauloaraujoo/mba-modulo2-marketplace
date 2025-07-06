import { ActivatedRoute } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';
import { Vendedor } from '../models/vendedor';
import { VendedorService } from '../services/vendedor.service';

@Component({
  selector: 'app-vendedor',
  styles: [],
  templateUrl: './vendedor.component.html'
})

export class VendedorComponent {

  @Input() vendedorId: string = "";

  public vendedor: Vendedor = new Vendedor();
    
  constructor(private route: ActivatedRoute,
              private vendedorService: VendedorService) { 

    this.route.params.subscribe(res => { this.vendedorId = res["id"]; });
  }

  ngOnInit() {
    this.vendedorService.obter(this.vendedorId)
      .subscribe({
        next: vendedor => this.vendedor = vendedor,
        error: error => console.error(error)
    }); 
  }  
}