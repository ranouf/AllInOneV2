import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../../../services/authentication/authentication.service';
import { TdLoadingService, tdHeadshakeAnimation, tdCollapseAnimation, tdFadeInOutAnimation } from '@covalent/core';
import { AccountService, FileParameter, UserDto } from '../../../services/api/api.services';
import { NotificationsService } from 'angular2-notifications';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
  animations: [
    tdHeadshakeAnimation,
    tdCollapseAnimation,
    tdFadeInOutAnimation,
  ],
})
export class ProfileComponent implements OnInit {
  public form: FormGroup;
  public errorMessage: string;
  public profileImage: FileParameter;
  public selectedProfileImage: string;
  public currentUser: UserDto;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private authenticationService: AuthenticationService,
    private loadingService: TdLoadingService,
    private notificationsService: NotificationsService,
  ) {
    this.form = this.fb.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required]
    });
  }

  reset() {
    this.errorMessage = null;
  }

  public async onFileChanged(event) {
    console.log('[image-upload]', event);
    let selectedFile = <File>event.target.files[0];
    this.profileImage = <FileParameter>{
      data: selectedFile,
      fileName: selectedFile.name
    };
    
    this.selectedProfileImage = await this.readFile(selectedFile);
  }

  public readFile(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      var fr = new FileReader();
      fr.onload = () => {
        resolve(fr.result.toString())
      };
      fr.readAsDataURL(file);
    });
  }

  remove() {
    this.selectedProfileImage = null;
  }

  update() {
    this.reset();
    this.loadingService.register();
    this.accountService.updateProfile(
      this.form.get('firstname').value,
      this.form.get('lastname').value,
      this.profileImage
    )
      .subscribe(response => {
        this.authenticationService.setCurrentUser(response);
        this.currentUser = this.authenticationService.currentUser.value
        this.remove();
        this.notificationsService.success(
          "Profile updated",
          "By " + this.authenticationService.currentUser.value.fullName
        );
        this.loadingService.resolve();
      }, error => {
        this.errorMessage = error.message
        console.error(error);
        this.loadingService.resolve();
      });
  }

  ngOnInit() {
    this.currentUser = this.authenticationService.currentUser.value
    this.form.patchValue({ firstname: this.currentUser.firstname });
    this.form.patchValue({ lastname: this.currentUser.lastname });
  }
}
